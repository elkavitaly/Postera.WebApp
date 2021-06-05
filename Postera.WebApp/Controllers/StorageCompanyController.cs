using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data;
using Postera.WebApp.Data.Interfaces;
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
        public async Task<IActionResult> GetStorageCompanies()
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var storageCompanies = await _adminService.GetStorageCompanies(token);

            return View("GetStorageCompaniesList", storageCompanies);
        }

        [HttpGet("/storageCompanys/json")]
        public async Task<IActionResult> GetStorageCompaniesList()
        {
            var storageCompanies = await _adminService.GetStorageCompanies();

            return Ok(storageCompanies);
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

        [HttpGet("/storageCompanys/{id}/modal")]
        public async Task<IActionResult> GetEditModal(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var storageCompany = await _adminService.GetStorageCompany(id, token);

            return View("GetStorageCompanyAddModal", storageCompany);
        }

        [HttpPost("/storageCompanys")]
        public async Task<IActionResult> AddStorageCompany(StorageCompany storageCompany)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var createdPostOffice = await _adminService.AddStorageCompany(storageCompany, token);

            return Ok(createdPostOffice.Id);
        }

        [HttpPut("/storageCompanys")]
        public async Task<IActionResult> EditPostOffice(StorageCompany storageCompany)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            await _adminService.EditStorageCompany(storageCompany, token);

            return Ok();
        }

        [HttpDelete("/storageCompanys/{id}")]
        public async Task<IActionResult> DeletePostOffice(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            await _adminService.DeleteStorageCompany(id, token);

            return Ok();
        }
    }
}