using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtakuVault.Data;

namespace OtakuVault.Controllers
{
    public class AdminController : Controller
    {
        private readonly OtakuVaultContext _context;

        public AdminController(OtakuVaultContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Index", "Home");

            var users = _context.UserAccount.ToList();
            var media = _context.MediaItem.ToList();

            ViewBag.Users = users;
            ViewBag.MediaItems = media;

            return View();
        }

        public async Task<IActionResult> DashboardStats()
        {
            var totalUsers = await _context.UserAccount.CountAsync();
            var totalAdmins = await _context.UserAccount.CountAsync(u => u.Role == "Admin");
            var totalMedia = await _context.MediaItem.CountAsync();

            var mediaTypeCounts = await _context.MediaItem
                .GroupBy(m => m.Type)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToListAsync();

            var topTracked = await _context.UserMediaStatus
                .GroupBy(s => s.MediaID)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new
                {
                    MediaTitle = _context.MediaItem.FirstOrDefault(m => m.Id == g.Key).Title,
                    Count = g.Count()
                })
                .ToListAsync();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalAdmins = totalAdmins;
            ViewBag.TotalMedia = totalMedia;
            ViewBag.MediaTypeCounts = mediaTypeCounts;
            ViewBag.TopTracked = topTracked;

            return View();
        }
    }
}
