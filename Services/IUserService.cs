using System.Collections.Generic;
using webapi.Entities;
using webapi.Models;

namespace webapi.Services
{
    public interface IUserService
    {
         User Authenticate(AuthenticateModel authenticate);
         List<User> allUsers();
    }
}