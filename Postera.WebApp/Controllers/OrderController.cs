﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data;
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

        [HttpGet("/users/orders/json")]
        public async Task<IActionResult> GetOrdersByUser([FromQuery]SelectParameters selectParameters)
        {
            string query = null;
            if (selectParameters.OrderBy != null || selectParameters.Skip != null || selectParameters.Take != null)
            {
                query = HttpContext.Request.QueryString.Value;
            }

            var token = ClaimsHelper.GetTokenFromClaims(User);
            var orders = await _adminService.GetOrders(token, query);

            return Ok(orders);
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
        public IActionResult CreateOrder(Order order)
        {
            //add order
            return View();
        }
    }
}