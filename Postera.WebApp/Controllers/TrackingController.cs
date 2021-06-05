using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data.Interfaces;

namespace Postera.WebApp.Controllers
{
    [Route("/tracking")]
    public class TrackingController : Controller
    {
        private readonly ITrackingService _trackingService;

        public TrackingController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> Tracking(string orderId)
        {
            var orderStatus = await _trackingService.GetOrderStatus(orderId);

            return View(orderStatus);
        }

        [HttpPost]
        public async Task<IActionResult> GetStatus(string orderId)
        {
            var orderStatus = await _trackingService.GetOrderStatus(orderId);

            return View(orderStatus);
        }
    }
}