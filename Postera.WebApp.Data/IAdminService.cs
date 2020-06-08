using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Postera.WebApp.Data.Enums;
using Postera.WebApp.Data.Models;

namespace Postera.WebApp.Data
{
    public interface IAdminService
    {
        Task<IList<Order>> GetOrders(Guid itemId, string itemType, string token, string query = null);

        Task<IList<Order>> GetOrders(string token, string query = null);

        Task<Order> GetOrder(Guid id, string token);

        Task AddOrder(Order order, string token);

        Task DeleteOrder(Guid id, string token);
        
        Task ChangeOrderStatus(Guid id, OrderStatus status, string token);

        Task<IList<Order>> SearchOrder(string searchValue, string token);

        Task<IList<PostOffice>> GetPostOffices(string token);

        Task<IList<PostOffice>> GetPostOffices();

        Task<PostOffice> GetPostOffice(Guid id, string token);

        Task<PostOffice> AddPostOffice(PostOffice postOffice, string token);

        Task EditPostOffice(PostOffice postOffice, string token);

        Task DeletePostOffice(Guid id, string token);

        Task BindStoragesToPostOffice(Guid id, Dictionary<string, string> param, string token);

        Task<IList<StorageCompany>> GetStorageCompanies(string token);

        Task<IList<StorageCompany>> GetStorageCompanies();

        Task<StorageCompany> GetStorageCompany(Guid id, string token);

        Task<IList<Storage>> GetStorages(Guid id, string type, string token);

        Task<Storage> GetStorage(Guid id, string token);

        Task<StorageCompany> AddStorageCompany(StorageCompany storageCompany, string token);

        Task EditStorageCompany(StorageCompany storageCompany, string token);

        Task DeleteStorageCompany(Guid id, string token);

        Task CreateStoragesCompany(Guid id, IList<Storage> storages, string token);

        Task<string> GetToken(LoginModel loginModel);

        Task<User> GetUser(string email, string token);

        Task<User> Register(RegisterModel registerModel);
    }
}