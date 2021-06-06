using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data.Interfaces;
using Postera.WebApp.Models;

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

        [HttpGet("")]
        public IActionResult Tracking()
        {
            return View();
        }

        [HttpPost("")]
        public async Task<IActionResult> OrderStatus(OrderStatusRequestModel model)
        {
            if (!Guid.TryParse(model.Id, out var id))
            {
                return RedirectToActionPermanent("Tracking");
            }

            var orderStatus = await _trackingService.GetOrderStatus(id);

            return View(orderStatus);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> OrderStatus(Guid orderId)
        {
            var orderStatus = await _trackingService.GetOrderStatus(orderId);

            return View(orderStatus);
        }
    }
}