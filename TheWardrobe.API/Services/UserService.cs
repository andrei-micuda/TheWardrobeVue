// using BCryptNet = BCrypt.Net.BCrypt;
// using System.Collections.Generic;
// using System.Linq;
// using TheWardrobe.API.Authorization;
// using TheWardrobe.API.Entities;
// using TheWardrobe.API.Helpers;
// using TheWardrobe.API.Models.Users;
// using Microsoft.Extensions.Configuration;
// using Npgsql;
// using Dapper;
// using AutoMapper;

// namespace TheWardrobe.API.Services
// {
//   public interface IUserService
//   {
//     AuthenticateResponse Authenticate(AuthenticateRequest model);
//     void Register(RegisterRequest model);
//   }

//   public class UserService : IUserService
//   {
//     private IJwtUtils _jwtUtils;
//     private readonly IMapper _mapper;

//     private readonly IConfiguration _configuration;

//     public UserService(IJwtUtils jwtUtils, IMapper mapper, IConfiguration configuration)
//     {
//       _jwtUtils = jwtUtils;
//       _mapper = mapper;
//       _configuration = configuration;
//     }

//     public AuthenticateResponse Authenticate(AuthenticateRequest model)
//     {
//       using (var connection = new NpgsqlConnection(_configuration["ConnectionString"]))
//       {
//         var user = connection.QuerySingleOrDefault<User>("SELECT * FROM Users WHERE Username = @username", new { username = model.Username });

//         // validate
//         // if (user == null || !BCryptNet.Verify(model.Password, user.PasswordHash))
//         //   throw new AppException("Username or password is incorrect");

//         // authentication successful
//         var response = _mapper.Map<AuthenticateResponse>(user);
//         response.JwtToken = _jwtUtils.GenerateToken(user);
//         return response;
//       }

//     }

//     public void Register(RegisterRequest model)
//     {
//       // validate
//       //if (_context.Users.Any(x => x.Username == model.Username))
//       //  throw new AppException("Username '" + model.Username + "' is already taken");

//       // map model to new user object
//       var user = _mapper.Map<User>(model);

//       // hash password
//       user.PasswordHash = BCryptNet.HashPassword(model.Password);

//       // save user
//       var sql = @"INSERT INTO Users (FirstName, LastName, Username, PasswordHash)
//                 VALUES (@FirstName, @LastName, @Username, @PasswordHash)";

//       using (var connection = new NpgsqlConnection(_configuration["ConnectionString"]))
//       {
//         connection.Execute(sql, user);
//       }
//     }
//   }
// }