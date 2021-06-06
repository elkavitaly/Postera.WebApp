using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data.Interfaces;
using Postera.WebApp.Data.Models;
using Postera.WebApp.Helpers;

namespace Postera.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var token = await _userService.GetToken(loginModel);
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

            await _userService.Register(credentials);

            return RedirectToAction("Login");
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

        [HttpGet("/users/{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var user = await _userService.GetUserByEmail(email, token);

            return Ok(user);
        }

        [HttpGet("/users")]
        public async Task<IActionResult> GetUserByName([FromQuery] string name)
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var users = await _userService.GetUsersByName(name, token);

            return Ok(users);
        }
    }
}