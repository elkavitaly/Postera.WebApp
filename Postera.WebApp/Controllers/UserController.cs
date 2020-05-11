using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data;
using Postera.WebApp.Data.Models;
using Postera.WebApp.Helpers;

namespace Postera.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IAdminService _adminService;

        public UserController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var token = await _adminService.GetToken(loginModel);
            var claimsPrincipal = ClaimsHelper.CreateTokenClaimsPrincipal(token);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return View(credentials);
            }

            await _adminService.Register(credentials);

            return RedirectToAction();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Account()
        {
            return View();
        }
    }
}