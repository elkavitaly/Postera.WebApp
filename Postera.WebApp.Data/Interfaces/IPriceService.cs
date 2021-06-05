using System.Threading.Tasks;
using Postera.WebApp.Data.Models;
using Postera.WebApp.Data.ResponseModels;

namespace Postera.WebApp.Data.Interfaces
{
    public interface IPriceService
    {
        Task<PriceResult> CalculatePrice(PriceModel priceModel);
    }
}