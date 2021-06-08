using Microsoft.AspNetCore.Mvc;

namespace Postera.WebApp.Controllers
{
    [Route("/packages")]
    public class PackageController : Controller
    {
        [HttpGet("section")]
        public IActionResult Section()
        {
            return View("GetPackageSection");
        }
    }
}