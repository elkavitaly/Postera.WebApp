using System.Collections.Generic;
using System.Threading.Tasks;
using Postera.WebApp.Data.Models;

namespace Postera.WebApp.Data.Interfaces
{
    public interface IUserService
    {
        Task<string> GetToken(LoginModel loginModel);

        Task<User> GetUserByEmail(string email, string token);

        Task<List<User>> GetUsersByName(string name, string token);

        Task<User> Register(RegisterModel registerModel);
    }
}