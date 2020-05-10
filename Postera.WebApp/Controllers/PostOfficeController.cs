using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Postera.WebApp.Data;
using Postera.WebApp.Data.Models;
using Postera.WebApp.Helpers;

namespace Postera.WebApp.Controllers
{
    [Authorize]
    public class PostOfficeController : Controller
    {
        private readonly IAdminService _adminService;

        public PostOfficeController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("/postOffices/template/list")]
        public IActionResult PostOfficesListTemplate()
        {
            return View();
        }

        [HttpGet("/postOffices")]
        public async Task<IActionResult> GetPostOfficesList()
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var postOffices = await _adminService.GetPostOffices(token);

            return View(postOffices);
        }

        [HttpGet("/postOffices/{id}/template")]
        public async Task<IActionResult> PostOfficeTemplate(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var postOffice = await _adminService.GetPostOffice(id, token);

            return View(postOffice);
        }

        [HttpGet("/postOffices/modal")]
        public IActionResult GetAddModal()
        {
            return View("GetPostOfficeAddModal");
        }

        [HttpGet("/postOffices/{id}")]
        public async Task<IActionResult> GetPostOffice(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var postOffice = await _adminService.GetPostOffice(id, token);

            return Ok(postOffice);
        }

        [HttpGet("/postOffices/{id}/modal")]
        public async Task<IActionResult> GetEditModal(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var postOffice = await _adminService.GetPostOffice(id, token);

            return View("GetPostOfficeAddModal", postOffice);
        }

        [HttpPost("/postOffices")]
        public async Task<IActionResult> AddPostOffice(PostOffice postOffice)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var createdPostOffice = await _adminService.AddPostOffice(postOffice, token);

            return Ok(createdPostOffice.Id);
        }

        [HttpPut("/postOffices")]
        public async Task<IActionResult> EditPostOffice(PostOffice postOffice)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            await _adminService.EditPostOffice(postOffice, token);

            return Ok();
        }

        [HttpDelete("/postOffices/{id}")]
        public async Task<IActionResult> DeletePostOffice(Guid id)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            await _adminService.DeletePostOffice(id, token);

            return Ok();
        }

        [HttpPost("/postOffices/{id}/storages")]
        public async Task<IActionResult> AddPostOffice(Guid id, [FromBody]Dictionary<string, string> param)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            await _adminService.BindStoragesToPostOffice(id, param, token);

            return Ok();
        }
    }
}