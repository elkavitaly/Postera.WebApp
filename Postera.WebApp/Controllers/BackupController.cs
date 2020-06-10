using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postera.WebApp.Data;
using Postera.WebApp.Helpers;

namespace Postera.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BackupController : Controller
    {
        private readonly IBackupService _backupService;

        public BackupController(IBackupService backupService)
        {
            _backupService = backupService;
        }

        [HttpGet]
        public IActionResult BackupPage()
        {
            return View("Backup");
        }

        [HttpPost]
        public async Task<IActionResult> Backup()
        {
            var token = ClaimsHelper.GetTokenFromClaims(User);
            var backup = await _backupService.Backup(token);

            return File(backup, "text/bak", "Backup.bak");
            
        }
    }
}