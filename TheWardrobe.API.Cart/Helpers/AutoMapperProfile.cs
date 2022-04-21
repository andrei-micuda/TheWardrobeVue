using AutoMapper;
using TheWardrobe.API.Cart.Entities;
using TheWardrobe.API.Cart.Models;

namespace TheWardrobe.API.Cart.Helpers
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<FavoritesRequest, Favorite>();
    }
  }
}