using System;
using System.Collections.Generic;
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

        [HttpGet("/items/{type}")]
        public IActionResult PostOfficesListTemplate(string type)
        {
            return View("PostOfficesListTemplate", type);
        }

        [HttpGet("/postOffices/list")]
        public async Task<IActionResult> GetPostOfficesList()
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var postOffices = await _adminService.GetPostOffices(token);

            return View(postOffices);
        }

        [HttpGet("/storageCompanys/list")]
        public async Task<IActionResult> GetStorageCompaniesList()
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var storageCompanies = await _adminService.GetStorageCompanies(token);

            return View(storageCompanies);
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

        [HttpGet("/orders/{id}/modal")]
        public async Task<IActionResult> GetOrderModal(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var order = await _adminService.GetOrder(id, token);

            return View(order);
        }

        [HttpGet("/postOffices/modal")]
        public IActionResult GetPostOfficeAddModal()
        {
            return View();
        }

        [HttpGet("/storageCompany/modal")]
        public IActionResult GetStorageCompanyAddModal()
        {
            return View();
        }

        [HttpGet("/postOffices/{id}")]
        public async Task<IActionResult> GetPostOffice(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var postOffice = await _adminService.GetPostOffice(id, token);

            return Ok(postOffice);
        }

        [HttpPost("/postOffices")]
        public async Task<IActionResult> AddPostOffice(PostOffice postOffice)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var createdPostOffice = await _adminService.AddPostOffice(postOffice, token);

            return Ok(createdPostOffice.Id);
        }

        [HttpPost("/storageCompany")]
        public async Task<IActionResult> AddStorageCompany(StorageCompany storageCompany)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var createdPostOffice = await _adminService.AddStorageCompany(storageCompany, token);

            return Ok(createdPostOffice.Id);
        }

        [HttpPost("/storageCompanies/{id}/storages")]
        public async Task<IActionResult> AddStoragesToCompany(Guid id, List<Storage> storages)
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

        [HttpGet("/storage/section")]
        public IActionResult GetStorageSection()
        {
            return View();
        }
    }
}