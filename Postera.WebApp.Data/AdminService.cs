using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Postera.WebApp.Data.Models;

namespace Postera.WebApp.Data
{
    public class AdminService : IAdminService
    {
        private readonly IHttpClient _client;

        public AdminService(IHttpClient client)
        {
            _client = client;
        }

        public async Task<IList<Order>> GetOrders(Guid itemId, string itemType, string token, string query = null)
        {
            var url = $"/api/admin/{itemType}s/{itemId}/orders";
            if (query != null)
            {
                url += query;
            }

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var orders = await _client.SendRequest<IList<Order>>(httpRequestMessage);

            return orders;
        }

        public async Task<Order> GetOrder(Guid id, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/orders/{id}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var order = await _client.SendRequest<Order>(httpRequestMessage);

            return order;
        }

        public async Task<IList<PostOffice>> GetPostOffices(string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/admin/postOffices");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var postOffices = await _client.SendRequest<IList<PostOffice>>(httpRequestMessage);

            return postOffices;
        }

        public async Task<PostOffice> GetPostOffice(Guid id, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/postOffices/{id}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var postOffice = await _client.SendRequest<PostOffice>(httpRequestMessage);

            return postOffice;
        }

        public async Task<IList<StorageCompany>> GetStorageCompanies(string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/admin/storageCompanies");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var storageCompanies = await _client.SendRequest<IList<StorageCompany>>(httpRequestMessage);

            return storageCompanies;
        }

        public async Task<StorageCompany> GetStorageCompany(Guid id, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/storageCompanies/{id}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var storageCompanies = await _client.SendRequest<StorageCompany>(httpRequestMessage);

            return storageCompanies;
        }

        public async Task<IList<StorageCompany>> GetStorages(Guid storageCompanyId, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/storageCompanies/{storageCompanyId}/storages");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var storageCompanies = await _client.SendRequest<IList<StorageCompany>>(httpRequestMessage);

            return storageCompanies;
        }

        public async Task<Storage> GetStorage(Guid id, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/storages/{id}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var storage = await _client.SendRequest<Storage>(httpRequestMessage);

            return storage;
        }

        public async Task<string> GetToken(LoginModel loginModel)
        {
            var serializedData = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(serializedData, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/users/token")
            {
                Content = content
            };

            var token = await _client.SendRequest<string>(httpRequestMessage);

            return token;
        }

        public Task<User> Register(RegisterModel registerModel)
        {
            throw new NotImplementedException();
        }
    }
}