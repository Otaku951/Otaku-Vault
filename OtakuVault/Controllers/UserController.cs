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
            // Ensure the user is logged in by checking session for UserID
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Retrieve the user's media status entries, including related media details
            var tracked = _context.UserMediaStatus
                .Include(ums => ums.Media)
                .Where(ums => ums.UserID == userId)
                .ToList();

            return View(tracked);
        }
    }
}
