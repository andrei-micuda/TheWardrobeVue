using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Text;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using TheWardrobe.Helpers;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace TheWardrobe.API.Repositories
{
  public interface IOrderRepository
  {
    OrderListResponse GetOrdersSummary(Guid accountId, OrderQueryFilters filters);
    void PlaceOrder(Guid accountId, OrderRequest model);
    OrderResponse GetOrder(Guid accountId, Guid orderId);
    IEnumerable<Guid> GetOrderItemIds(Guid orderId);
    Guid GetBuyerId(Guid orderId);
    Guid GetSellerId(Guid orderId);
    bool UpdateOrderStatus(Guid accountId, Guid orderId, OrderStatus status);
    bool ReviewOrder(Guid accountId, Guid orderId, int rating);
    float? GetBuyerRating(Guid buyerId);
    float? GetSellerRating(Guid sellerId);
  }

  public class OrderRepository : IOrderRepository
  {
    private readonly IMapper _mapper;
    private readonly IDapperContext _dapperContext;

    public OrderRepository(IMapper mapper, IDapperContext dapperContext)
    {
      _mapper = mapper;
      _dapperContext = dapperContext;
    }

    public void PlaceOrder(Guid accountId, OrderRequest model)
    {
      var order = _mapper.Map<Entities.Order>(model);
      order.BuyerId = accountId;
      order.WhenPlaced = DateTime.UtcNow;

      using var connection = _dapperContext.GetConnection();
      connection.Open();
      using var tx = connection.BeginTransaction();

      try
      {
        var orderId = connection.Insert<Guid, Entities.Order>(order);
        var orderItems = model.Items.Select(id => new
        {
          ItemId = id,
          OrderId = orderId
        });

        foreach (var orderItem in orderItems)
        {
          connection.Execute(@"
            INSERT INTO order_item(item_id, order_id)
            VALUES (@ItemId, @OrderId)", orderItem);
        }

        tx.Commit();
      }
      catch (Exception)
      {
        tx.Rollback();
        throw new AppException("Order could not be placed.");
      }
      finally
      {
        connection.Close();
      }
    }

    public OrderListResponse GetOrdersSummary(Guid accountId, OrderQueryFilters filters)
    {
      var res = new OrderListResponse();
      using var connection = _dapperContext.GetConnection();

      // maps valid order by query parameters to the sql column
      var orderByColumns = new Dictionary<string, string>()
      {
        {"whenPlaced", "when_placed"},
      };

      if (!orderByColumns.ContainsKey(filters.OrderBy))
      {
        throw new AppException("Invalid orderBy value.");
      }
      var order = filters.Order.ToUpper();
      if (order != "ASC" && order != "DESC")
      {
        throw new AppException("Invalid order value.");
      }

      var orderByColumn = orderByColumns[filters.OrderBy];

      res.Orders = connection.Query<OrderResponse>(@$"
        SELECT *
        FROM ""order""
        WHERE {(filters.Type == OrderType.Incoming ? "buyer_id" : "seller_id")} = @accountId
        ORDER BY {orderByColumn} {order}
        LIMIT @limit
        OFFSET @offset;
      ", new
      {
        accountId,
        limit = filters.PageSize,
        offset = filters.PageSize * (filters.Page - 1)
      });

      foreach (var orderResponse in res.Orders)
      {
        orderResponse.Total = connection.ExecuteScalar<float>(@"
          SELECT SUM(i.price)
          FROM item i
          WHERE i.id IN (
            SELECT item_id
            FROM order_item
            WHERE order_id = @OrderId)", new { OrderId = orderResponse.Id });
      }

      res.NumOrders = connection.ExecuteScalar<int>(@$"
        SELECT COUNT(*)
        FROM ""order""
        WHERE {(filters.Type == OrderType.Incoming ? "buyer_id" : "seller_id")} = @accountId
        ", new
      {
        accountId,
      });

      return res;
    }

    public OrderResponse GetOrder(Guid accountId, Guid orderId)
    {
      var buyerId = GetBuyerId(orderId);
      var sellerId = GetSellerId(orderId);

      if (accountId != buyerId && accountId != sellerId)
        return null;

      using var connection = _dapperContext.GetConnection();

      var order = connection.QueryFirstOrDefault<Entities.Order>(@$"
        SELECT *
        FROM ""order""
        WHERE id = @orderId;
      ", new { orderId });

      var res = _mapper.Map<OrderResponse>(order);
      res.DeliveryAddress = new();
      res.DeliveryAddress.Id = order.DeliveryAddressId;

      res.BuyerRating = GetBuyerRating(buyerId);
      res.SellerRating = GetSellerRating(sellerId);

      (res.OrderBuyerRating, res.OrderSellerRating) = GetOrderReview(accountId, orderId);

      return res;
    }

    public IEnumerable<Guid> GetOrderItemIds(Guid orderId)
    {
      using var connection = _dapperContext.GetConnection();
      return connection.Query<Guid>(@"
        SELECT item_id
        FROM order_item
        WHERE order_id = @orderId", new { orderId });
    }

    public Guid GetBuyerId(Guid orderId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.ExecuteScalar<Guid>(@"SELECT buyer_id FROM ""order"" WHERE id = @orderId", new { orderId });
    }

    public Guid GetSellerId(Guid orderId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.ExecuteScalar<Guid>(@"SELECT seller_id FROM ""order"" WHERE id = @orderId", new { orderId });
    }

    private void SetOrderStatus(Guid orderId, OrderStatus status)
    {
      using var connection = _dapperContext.GetConnection();

      connection.Execute(@"UPDATE ""order"" SET status = @status WHERE id = @orderId", new { orderId, status });
    }

    private OrderStatus GetOrderStatus(Guid orderId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.ExecuteScalar<OrderStatus>(@"
        SELECT status
        FROM ""order""
        WHERE id = @orderId", new { orderId });
    }

    public bool UpdateOrderStatus(Guid accountId, Guid orderId, OrderStatus status)
    {
      using var connection = _dapperContext.GetConnection();

      var buyerId = GetBuyerId(orderId);
      var sellerId = GetSellerId(orderId);

      var currentStatus = GetOrderStatus(orderId);

      connection.Open();
      var tx = connection.BeginTransaction();

      if (status == OrderStatus.Pending)
      {
        // pending status shall not be set through the API
        return false;
      }

      if (status == OrderStatus.InProgress)
      {
        // only seller can accept order
        if (currentStatus != OrderStatus.Pending || accountId != sellerId)
          return false;

        var orderItemIds = GetOrderItemIds(orderId);

        // remove items from user carts
        connection.Execute(@"
          DELETE FROM cart
          WHERE item_id = ANY(@orderItemIds);", new { orderItemIds });

        // decline all other pending orders that contain any of the items in the accepted order
        var otherOrderIds = connection.Query<Guid>(@"
          SELECT DISTINCT order_id
          FROM order_item
          WHERE order_id != @orderId
          AND item_id = ANY(@orderItemIds)", new { orderId, orderItemIds });

        foreach (var id in otherOrderIds)
        {
          SetOrderStatus(id, OrderStatus.Declined);
        }
      }

      if (status == OrderStatus.Completed)
      {
        // only buyer can confirm order completion
        if (currentStatus != OrderStatus.InProgress || accountId != buyerId)
          return false;

        // create entry for order reviews
        connection.Execute(@"
          INSERT INTO review (order_id)
          VALUES (@orderId);", new { orderId });
      }

      if (status == OrderStatus.Cancelled)
      {
        // only buyer can cancel an order
        if (currentStatus != OrderStatus.Pending || accountId != buyerId)
          return false;
      }

      if (status == OrderStatus.Declined)
      {
        // only seller can cancel an order
        if (currentStatus != OrderStatus.Pending || accountId != sellerId)
          return false;
      }

      SetOrderStatus(orderId, status);

      tx.Commit();
      return true;
    }

    public bool ReviewOrder(Guid accountId, Guid orderId, int rating)
    {
      var buyerId = GetBuyerId(orderId);
      var sellerId = GetSellerId(orderId);

      var orderStatus = GetOrderStatus(orderId);

      using var connection = _dapperContext.GetConnection();

      if (orderStatus != OrderStatus.Completed)
        return false;

      if (accountId == buyerId)
      {
        connection.Execute(@"
          UPDATE review
          SET seller_rating = @rating
          WHERE order_id = @orderId;", new { rating, orderId });
      }
      else if (accountId == sellerId)
      {
        connection.Execute(@"
          UPDATE review
          SET buyer_rating = @rating
          WHERE order_id = @orderId;", new { rating, orderId });
      }
      else
      {
        return false;
      }

      return true;
    }

    private (int, int) GetOrderReview(Guid accountId, Guid orderId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.QueryFirstOrDefault<(int, int)>(@"
        SELECT buyer_rating, seller_rating
        FROM review
        WHERE order_id = @orderId;", new { orderId });
    }

    public float? GetSellerRating(Guid sellerId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.ExecuteScalar<float?>(@"
        SELECT AVG(r.seller_rating)
        FROM review r, ""order"" o
        WHERE r.order_id = o.id
        AND o.seller_id = @sellerId;", new { sellerId });
    }

    public float? GetBuyerRating(Guid buyerId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.ExecuteScalar<float?>(@"
        SELECT AVG(r.buyer_rating)
        FROM review r, ""order"" o
        WHERE r.order_id = o.id
        AND o.buyer_id = @buyerId;", new { buyerId });
    }
  }
}