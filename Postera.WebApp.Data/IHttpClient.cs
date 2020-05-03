using System.Net.Http;
using System.Threading.Tasks;

namespace Postera.WebApp.Data
{
    public interface IHttpClient
    {
        Task<T> SendRequest<T>(HttpRequestMessage message);
    }
}