using AutoMapper;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Models;

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