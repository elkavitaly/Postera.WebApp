using System;
using System.Threading.Tasks;
using Postera.WebApp.Data.Models;

namespace Postera.WebApp.Data.Interfaces
{
    public interface ITrackingService
    {
        Task<Tracking> GetOrderStatus(Guid orderId);
    }
}