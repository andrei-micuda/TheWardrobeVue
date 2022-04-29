using AutoMapper;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Models;

namespace TheWardrobe.API.Order.Helpers
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<FavoritesRequest, Favorite>();
      CreateMap<CartRequest, Cart>();
      CreateMap<OrderRequest, Entities.Order>();
      CreateMap<Entities.Order, OrderResponse>();
    }
  }
}