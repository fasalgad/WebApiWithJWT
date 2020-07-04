using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Entities;
using webapi.Helpers;
using webapi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace webapi.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        private List<User> _users = new List<User>
        {
            new User {id = 1, firstname = "Test", firstsurname = "User", username = "test", password = "test" },
            new User {id = 2, firstname = "Test 2", firstsurname = "User 2", username = "test2", password = "test2" }
        };
        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public List<User> allUsers()
        {
           return _users;
        }

        public User Authenticate(AuthenticateModel userario)
        {
            // var user = new User { id = 1, firstname = "Test", firstsurname = "User", username = "test", password = "test" };
            var user =_users.Where(x => x.username==userario.username && x.password==userario.password).FirstOrDefault();
            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Role,"Estudiante")
                }),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            user.token = tokenString;
            return user.WithoutPassword();
        }


    }
}