using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Postera.WebApp.Data.Models;

namespace Postera.WebApp.Data
{
    public class AdminService
    {
        private readonly IHttpClient _client;

        public AdminService(IHttpClient client)
        {
            _client = client;
        }

        public async Task<IList<Order>> GetOrders(Guid itemId, string itemType, string token)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/{itemType}s/{itemId}/orders");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var orders = await _client.SendRequest<IList<Order>>(httpRequestMessage);

            return orders;
        }


    }
}