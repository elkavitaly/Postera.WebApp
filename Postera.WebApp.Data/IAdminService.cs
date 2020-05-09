using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Postera.WebApp.Data.Models;

namespace Postera.WebApp.Data
{
    public interface IAdminService
    {
        Task<IList<Order>> GetOrders(Guid itemId, string itemType, string token, string query = null);

        Task<Order> GetOrder(Guid id, string token);

        Task<IList<PostOffice>> GetPostOffices(string token);

        Task<PostOffice> GetPostOffice(Guid id, string token);

        Task<PostOffice> AddPostOffice(PostOffice postOffice, string token);

        Task EditPostOffice(PostOffice postOffice, string token);
        
        Task DeletePostOffice(Guid id, string token);

        Task<IList<StorageCompany>> GetStorageCompanies(string token);

        Task<StorageCompany> GetStorageCompany(Guid id, string token);

        Task<IList<Storage>> GetStorages(Guid id, string type, string token);

        Task<Storage> GetStorage(Guid id, string token);

        Task<StorageCompany> AddStorageCompany(StorageCompany storageCompany, string token);

        Task CreateStoragesCompany(Guid id, IList<Storage> storages, string token);

        Task<string> GetToken(LoginModel loginModel);

        Task<User> Register(RegisterModel registerModel);
    }
}