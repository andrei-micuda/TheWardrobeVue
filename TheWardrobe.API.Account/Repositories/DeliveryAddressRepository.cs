using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Models;
using TheWardrobe.Helpers;

namespace TheWardrobe.API.Repositories
{
  public interface IDeliveryAddressRepository
  {
    DeliveryAddress AddDeliveryAddress(Guid accountId, DeliveryAddressRequestResponse model);
    DeliveryAddress Get(Guid deliveryAddressId);
    IEnumerable<DeliveryAddress> GetAll(Guid accountId);
    void DeleteDeliveryAddress(Guid accountId, Guid deliveryAddressId);
    DeliveryAddress UpdateDeliveryAddress(Guid accountId, Guid deliveryAddressId, DeliveryAddressRequestResponse model);
  }

  public class DeliveryAddressRepository : IDeliveryAddressRepository
  {
    private readonly IMapper _mapper;
    private readonly IDapperContext _dapperContext;

    public DeliveryAddressRepository(IMapper mapper, IDapperContext dapperContext)
    {
      _mapper = mapper;
      _dapperContext = dapperContext;
    }
    public DeliveryAddress AddDeliveryAddress(Guid accountId, DeliveryAddressRequestResponse model)
    {
      var address = _mapper.Map<DeliveryAddress>(model);
      address.AccountId = accountId;
      using var connection = _dapperContext.GetConnection();

      connection.Insert<Guid, DeliveryAddress>(address);

      return address;
    }

    public DeliveryAddress Get(Guid deliveryAddressId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.Get<DeliveryAddress>(deliveryAddressId);
    }

    public IEnumerable<DeliveryAddress> GetAll(Guid accountId)
    {
      using var connection = _dapperContext.GetConnection();

      return connection.GetList<DeliveryAddress>(new { AccountId = accountId });
    }

    public void DeleteDeliveryAddress(Guid accountId, Guid deliveryAddressId)
    {
      using var connection = _dapperContext.GetConnection();

      connection.Execute(@"
        DELETE FROM delivery_address
        WHERE id = @deliveryAddressId
        AND account_id = @accountId;
      ", new { deliveryAddressId, accountId });
    }

    public DeliveryAddress UpdateDeliveryAddress(Guid accountId, Guid deliveryAddressId, DeliveryAddressRequestResponse model)
    {
      var address = _mapper.Map<DeliveryAddress>(model);
      address.Id = deliveryAddressId;
      address.AccountId = accountId;

      using var connection = _dapperContext.GetConnection();
      connection.Update(address);
      return address;
    }
  }
}