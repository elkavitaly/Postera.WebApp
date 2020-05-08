using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Postera.WebApp.Data
{
    public class HttpClient : IHttpClient
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> SendRequest<T>(HttpRequestMessage message) where T : class
        {
            var client = _clientFactory.CreateClient("default");

            var responseMessage = await client.SendAsync(message);

            var content = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RequestException(responseMessage.StatusCode, content);
            }

            if (typeof(T) == typeof(string))
            {
                return content as T;
            }

            var result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }
    }
}