using AutoMapper;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Models;

namespace TheWardrobe.API.ItemCatalog.Helpers
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<ItemRequestResponse, Item>();
    }
  }
}