using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Postera.WebApp.Data.Interfaces;
using Postera.WebApp.Data.Models;
using Postera.WebApp.Data.ResponseModels;

namespace Postera.WebApp.Data.Services
{
    public class PriceService : IPriceService
    {
        private readonly IHttpClient _httpClient;

        public PriceService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PriceResult> CalculatePrice(PriceModel priceModel)
        {
            var serializedOrder = JsonConvert.SerializeObject(priceModel);
            var content = new StringContent(serializedOrder, Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/prices")
            {
                Content = content
            };

            var priceResult = await _httpClient.SendRequest<Result<PriceResult>>(httpRequestMessage);

            return priceResult.Data;
        }
    }
}