using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Postera.WebApp.Data
{
    public class BackupService : IBackupService
    {
        private readonly IHttpClientFactory _clientFactory;

        public BackupService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<byte[]> Backup(string token)
        {
            var client = _clientFactory.CreateClient("default");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/backup");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var responseMessage = await client.SendAsync(httpRequestMessage);

            var readAsByteArrayAsync = await responseMessage.Content.ReadAsByteArrayAsync();

            return readAsByteArrayAsync;
        }
    }
}