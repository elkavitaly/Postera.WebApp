using System.Net.Http;
using System.Threading.Tasks;
using Postera.WebApp.Data.Interfaces;
using Postera.WebApp.Data.Models;

namespace Postera.WebApp.Data.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly IHttpClient _httpClient;

        public TrackingService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Tracking> GetOrderStatus(string orderId)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/tracking/{orderId}");

            var priceResult = await _httpClient.SendRequest<Result<Tracking>>(httpRequestMessage);

            return priceResult.Data;
        }
    }
}