using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data.Interfaces;
using Postera.WebApp.Data.Models;

namespace Postera.WebApp.Controllers
{
    [Route("/price")]
    public class PriceController : Controller
    {
        private readonly IPriceService _priceService;
        private readonly IAdminService _adminService;

        public PriceController(IPriceService priceService, IAdminService adminService)
        {
            _priceService = priceService;
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult CalculatePrice()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CalculatePrice(PriceModel priceModel)
        {
            var priceResult = await _priceService.CalculatePrice(priceModel);

            return View("CalculatePriceResult", priceResult);
        }

        [HttpGet("packages/section")]
        public IActionResult GetPackageSection()
        {
            return View();
        }
    }
}