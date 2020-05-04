using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data;
using Postera.WebApp.Data.Models;
using Postera.WebApp.Helpers;

namespace Postera.WebApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("/postOffices")]
        public IActionResult PostOfficesListTemplate()
        {
            return View();
        }

        [HttpGet("/postOffices/list")]
        public async Task<IActionResult> GetPostOfficesList()
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var postOffices = await _adminService.GetPostOffices(token);

            return View(postOffices);
        }

        [HttpGet]
        public async Task<IActionResult> PostOfficeTemplate(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var postOffice = await _adminService.GetPostOffice(id, token);

            return View(postOffice);
        }

        [HttpGet("/{type}s/{itemId}/orders")]
        public async Task<IActionResult> GetOrders(string type, Guid itemId, [FromQuery]SelectParameters selectParameters)
        {
            string query = null;
            if (selectParameters.OrderBy != null || selectParameters.Skip != null || selectParameters.Take != null)
            {
                query = HttpContext.Request.QueryString.Value;
            }

            var token = ClaimsHelper.GetTokenFromClaims(User);
            var orders = await _adminService.GetOrders(itemId, type, token, query);

            return View(orders);
        }

        [HttpGet("/orders/{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var order = await _adminService.GetOrder(id, token);

            return Ok(order);
        }
        
        [HttpGet("/postOffices/{id}")]
        public async Task<IActionResult> GetPostOffice(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var postOffice = await _adminService.GetPostOffice(id, token);

            return Ok(postOffice);
        }
        
        [HttpGet("/storages/{id}")]
        public async Task<IActionResult> GetStorage(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var storage = await _adminService.GetStorage(id, token);

            return Ok(storage);
        }

        [HttpGet]
        public IActionResult OrdersTemplate(Guid id)
        {
            return View(id);
        }
    }
}