using Microsoft.AspNetCore.Mvc;
using OtakuVault.Data;
using OtakuVault.Models;
using System.Diagnostics;

namespace OtakuVault.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OtakuVaultContext _context;

        public HomeController(ILogger<HomeController> logger, OtakuVaultContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            // Pull latest media
            var latestMedia = _context.MediaItem
                .Where(m => m.DateAdded.Date == today || m.DateAdded.Date == yesterday)
                .OrderByDescending(m => m.DateAdded)
                .ToList();

            return View(latestMedia);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
