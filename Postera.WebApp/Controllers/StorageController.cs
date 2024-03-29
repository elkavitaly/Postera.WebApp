﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data;
using Postera.WebApp.Data.Interfaces;
using Postera.WebApp.Data.Models;
using Postera.WebApp.Helpers;
using Postera.WebApp.Models;

namespace Postera.WebApp.Controllers
{
    [Authorize]
    public class StorageController : Controller
    {
        private readonly IAdminService _adminService;

        public StorageController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("/{type}/{id}/storages/template/list")]
        public IActionResult StoragesListTemplate(SearchParameters parameters)
        {
            return View(parameters);
        }

        [HttpPost("/storageCompanies/{id}/storages")]
        public async Task<IActionResult> AddStoragesToCompany(Guid id, List<Storage> storages)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            await _adminService.CreateStoragesCompany(id, storages, token);

            return Ok();
        }
        
        [HttpGet("/{type}/{id}/storages")]
        public async Task<IActionResult> GetStoragesList([FromRoute]SearchParameters parameters)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var storageCompanies = await _adminService.GetStorages(parameters.Id, parameters.Type);

            return View(storageCompanies);
        }

        [AllowAnonymous]
        [HttpGet("/{type}/{id}/storages/json")]
        public async Task<IActionResult> GetStorages([FromRoute]SearchParameters parameters)
        {
            var storageCompanies = await _adminService.GetStorages(parameters.Id, parameters.Type);

            return Ok(storageCompanies);
        }

        [HttpGet("/storages/{id}")]
        public async Task<IActionResult> GetStorage(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var storage = await _adminService.GetStorage(id, token);

            return Ok(storage);
        }

        [HttpGet("/storages/section")]
        public IActionResult GetStorageSection()
        {
            return View();
        }

        [HttpGet("/storages/postOffices/{id}/modal")]
        public IActionResult GetPostOfficeBindModal([FromRoute] SearchParameters searchParameters)
        {
            return View("GetStorageToPostOfficeAddModal", searchParameters);
        }

        [HttpGet("/storages/storageCompanys/{id}/modal")]
        public IActionResult GetStorageCompanyBindModal([FromRoute] SearchParameters searchParameters)
        {
            return View("GetStorageToStorageCompanyAddModal");
        }
    }
}