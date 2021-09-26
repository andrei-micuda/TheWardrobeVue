using AutoMapper;
using TheWardrobe.API.Entities;
using TheWardrobe.API.Models.Users;

namespace TheWardrobe.API.Helpers
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      // User -> AuthenticateResponse
      CreateMap<User, AuthenticateResponse>();

      // RegisterRequest -> User
      CreateMap<RegisterRequest, User>();
    }
  }
}