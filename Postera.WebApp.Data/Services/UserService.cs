using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Postera.WebApp.Data.Interfaces;
using Postera.WebApp.Data.Models;

namespace Postera.WebApp.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClient _httpClient;

        public UserService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetToken(LoginModel loginModel)
        {
            var serializedData = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(serializedData, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/users/token")
            {
                Content = content
            };

            var token = await _httpClient.SendRequest<string>(httpRequestMessage);

            return token;
        }

        public async Task<User> GetUserByEmail(string email, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/users/{email}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var user = await _httpClient.SendRequest<User>(httpRequestMessage);

            return user;
        }

        public async Task<List<User>> GetUsersByName(string name, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/users?search={name}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.SendRequest<Result<List<User>>>(httpRequestMessage);

            return result.IsSuccess
                ? result.Data ?? new List<User>()
                : new List<User>();
        }

        public async Task<User> Register(RegisterModel registerModel)
        {
            var serializedStorages = JsonConvert.SerializeObject(registerModel);
            var content = new StringContent(serializedStorages, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/api/users/register")
            {
                Content = content
            };

            var user = await _httpClient.SendRequest<User>(httpRequestMessage);

            return user;
        }
    }
}