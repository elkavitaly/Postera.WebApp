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
    public class StorageCompanyController : Controller
    {
        private readonly IAdminService _adminService;

        public StorageCompanyController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("/storageCompanys/template/list")]
        public IActionResult StorageCompanysListTemplate()
        {
            return View();
        }

        [HttpGet("/storageCompanys")]
        public async Task<IActionResult> GetStorageCompaniesList()
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var storageCompanies = await _adminService.GetStorageCompanies(token);

            return View(storageCompanies);
        }

        [HttpGet("/storageCompanys/{id}/template")]
        public async Task<IActionResult> StorageCompanyTemplate(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var storageCompany = await _adminService.GetStorageCompany(id, token);

            return View(storageCompany);
        }

        [HttpGet("/storageCompanys/modal")]
        public IActionResult GetStorageCompanyAddModal()
        {
            return View();
        }

        [HttpPost("/storageCompanys")]
        public async Task<IActionResult> AddStorageCompany(StorageCompany storageCompany)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var createdPostOffice = await _adminService.AddStorageCompany(storageCompany, token);

            return Ok(createdPostOffice.Id);
        }
    }
}