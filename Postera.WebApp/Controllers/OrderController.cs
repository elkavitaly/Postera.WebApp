using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data;
using Postera.WebApp.Data.Enums;
using Postera.WebApp.Data.Models;
using Postera.WebApp.Helpers;
using Postera.WebApp.Models;

namespace Postera.WebApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IAdminService _adminService;

        public OrderController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("/{type}/{itemId}/orders")]
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

        [HttpGet("/{type}/{itemId}/orders/json")]
        public async Task<IActionResult> GetOrdersList(Guid itemId, string type, [FromQuery]SelectParameters selectParameters)
        {
            string query = null;
            if (selectParameters.OrderBy != null || selectParameters.Skip != null || selectParameters.Take != null)
            {
                query = HttpContext.Request.QueryString.Value;
            }

            var token = ClaimsHelper.GetTokenFromClaims(User);
            var orders = await _adminService.GetOrders(itemId, type, token, query);

            return Ok(orders);
        }

        [HttpGet("/users/orders")]
        public async Task<IActionResult> GetOrdersByUser([FromQuery]SelectParameters selectParameters)
        {
            string query = null;
            if (selectParameters.OrderBy != null || selectParameters.Skip != null || selectParameters.Take != null)
            {
                query = HttpContext.Request.QueryString.Value;
            }

            var token = ClaimsHelper.GetTokenFromClaims(User);
            var orders = await _adminService.GetOrders(token, query);

            return View(orders);
        }

        [HttpGet("/orders/{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var order = await _adminService.GetOrder(id, token);

            return Ok(order);
        }

        [HttpGet("/orders/{id}/modal")]
        public async Task<IActionResult> GetOrderModal(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var order = await _adminService.GetOrder(id, token);

            return View(order);
        }

        [HttpGet("/{type}/{id}/orders/template")]
        public IActionResult OrdersTemplate(SearchParameters parameters)
        {
            return View(parameters);
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            await _adminService.AddOrder(order, token);

            return RedirectToAction("Account", "User");
        }

        [HttpDelete("/orders/{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            await _adminService.DeleteOrder(id, token);

            return Ok();
        }
        
        [HttpPost("/orders/{id}/{status}")]
        public async Task<IActionResult> ChangeStatus(Guid id, OrderStatus status)
        {
            return Ok();
        }
    }
}