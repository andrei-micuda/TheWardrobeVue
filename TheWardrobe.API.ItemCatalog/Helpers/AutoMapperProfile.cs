using AutoMapper;
using TheWardrobe.API.ItemCatalog.Entities;
using TheWardrobe.API.ItemCatalog.Models;

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