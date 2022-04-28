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
using TheWardrobe.CrossCutting.Helpers;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Models;

namespace TheWardrobe.API.Repositories
{
  public interface IOrderRepository
  {
    OrderListResponse GetOrdersSummary(Guid accountId, OrderQueryFilters filters);
    void PlaceOrder(Guid accountId, OrderRequest model);
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
      catch (Exception ex)
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
        AND status = @Status
        ORDER BY {orderByColumn} {order}
        LIMIT @limit
        OFFSET @offset;
      ", new
      {
        accountId,
        filters.Status,
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
        AND status = @Status;
        ", new
      {
        accountId,
        filters.Status,
      });

      return res;
    }
  }
}