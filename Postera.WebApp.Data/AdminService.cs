using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Postera.WebApp.Data.Helpers;
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
            var url = $"/api/admin/{itemType}/{itemId}/orders";
            if (query != null)
            {
                url += query;
            }

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var orders = await _client.SendRequest<IList<Order>>(httpRequestMessage);

            return orders;
        }

        public async Task<IList<Order>> GetOrders(string token, string query = null)
        {
            var url = "/api/users/orders";
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

        public async Task<IList<PostOffice>> GetPostOffices()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/postOffices");

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

        public async Task<PostOffice> AddPostOffice(PostOffice postOffice, string token)
        {
            var serializedPostOffice = JsonConvert.SerializeObject(postOffice);
            var content = new StringContent(serializedPostOffice, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/postOffices/");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequestMessage.Content = content;

            var createdPostOffice = await _client.SendRequest<PostOffice>(httpRequestMessage);

            return createdPostOffice;
        }

        public async Task EditPostOffice(PostOffice postOffice, string token)
        {
            var serializedPostOffice = JsonConvert.SerializeObject(postOffice);
            var content = new StringContent(serializedPostOffice, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/postOffices/");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequestMessage.Content = content;

            await _client.SendRequest<string>(httpRequestMessage);
        }

        public async Task DeletePostOffice(Guid id, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/api/postOffices/{id}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await _client.SendRequest<string>(httpRequestMessage);
        }

        public async Task BindStoragesToPostOffice(Guid id, Dictionary<string, string> param, string token)
        {
            var parameters = string.Empty;
            foreach (var keyPair in param)
            {
                parameters += $"{keyPair.Key}:{keyPair.Value},";
            }

            parameters.Trim(',');
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/api/postOffices/{id}/storages?param={parameters}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await _client.SendRequest<string>(httpRequestMessage);
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

        public async Task<IList<Storage>> GetStorages(Guid id, string type, string token)
        {
            if (RouteMapper.RouteValues.TryGetValue(type, out var routeType))
            {
                type = routeType;
            }

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/{type}/{id}/storages");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var storages = await _client.SendRequest<IList<Storage>>(httpRequestMessage);

            return storages;
        }

        public async Task<Storage> GetStorage(Guid id, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/storages/{id}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var storage = await _client.SendRequest<Storage>(httpRequestMessage);

            return storage;
        }

        public async Task<StorageCompany> AddStorageCompany(StorageCompany storageCompany, string token)
        {
            var serializedStorageCompany = JsonConvert.SerializeObject(storageCompany);
            var content = new StringContent(serializedStorageCompany, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/storageCompanies/");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequestMessage.Content = content;

            var createdStorageCompany = await _client.SendRequest<StorageCompany>(httpRequestMessage);

            return createdStorageCompany;
        }

        public async Task EditStorageCompany(StorageCompany storageCompany, string token)
        {
            var serializedPostOffice = JsonConvert.SerializeObject(storageCompany);
            var content = new StringContent(serializedPostOffice, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/storageCompanies/");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequestMessage.Content = content;

            await _client.SendRequest<string>(httpRequestMessage);
        }

        public async Task DeleteStorageCompany(Guid id, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/api/storageCompanies/{id}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await _client.SendRequest<string>(httpRequestMessage);
        }

        public async Task CreateStoragesCompany(Guid id, IList<Storage> storages, string token)
        {
            var serializedStorages = JsonConvert.SerializeObject(storages);
            var content = new StringContent(serializedStorages, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/api/storageCompanies/{id}/storages");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequestMessage.Content = content;

            await _client.SendRequest<string>(httpRequestMessage);
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