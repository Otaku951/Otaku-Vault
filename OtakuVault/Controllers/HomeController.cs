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
                .Where(m =>m.DateAdded.Date >= yesterday && m.DateAdded.Date <= today // Media item itself added within the time range 
                    || m.Entries.Any(e => e.ReleaseDate.Date >= yesterday && e.ReleaseDate.Date <= today)) // or any of its entries added within the time range
                .OrderByDescending(m => new[] { m.DateAdded } // Order by media item date or entry date
                    .Concat(m.Entries.Select(e => e.ReleaseDate))
                    .Max()) 
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
