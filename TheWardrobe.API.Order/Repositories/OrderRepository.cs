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

      // foreach (var o in res.Orders)
      // {
      //   o.Buyer = connection.ExecuteScalar<stringasdfda>(@"
      //    SELECT COALESCE(first_name || ' ' || last_name, email)
      //    FROM account
      //    WHERE id = @BuyerId", o);

      //   o.Seller = connection.ExecuteScalar<string>(@"
      //    SELECT COALESCE(first_name || ' ' || last_name, email)
      //    FROM account
      //    WHERE id = @SellerId", o);
      // }

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