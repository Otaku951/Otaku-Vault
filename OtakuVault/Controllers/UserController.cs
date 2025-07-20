using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtakuVault.Data;
using System.Text;

namespace OtakuVault.Controllers
{
    public class UserController : Controller
    {
        private readonly OtakuVaultContext _context;

        public UserController(OtakuVaultContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReadingList()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var tracked = _context.UserMediaStatus
                .Include(ums => ums.Media)
                .Where(ums => ums.UserID == userId)
                .ToList();

            return View(tracked);
        }
    }
}
