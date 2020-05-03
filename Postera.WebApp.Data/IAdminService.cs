using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Postera.WebApp.Data.Models;

namespace Postera.WebApp.Data
{
    public interface IAdminService
    {
        Task<IList<Order>> GetOrders(Guid itemId, string itemType, string token);

        Task<string> GetToken(string email, string password);

        Task<User> Register(RegisterModel registerModel);
    }
}