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
            var mediaItems = _context.MediaItem.ToList();
            var mediaEntries = _context.MediaEntry.ToList();
            var coinPackages = _context.CoinPackages.ToList();
            var transactions = _context.Transactions.ToList();
            var entryUnlocks = _context.EntryUnlocks.ToList();

            ViewBag.Users = users;
            ViewBag.MediaItems = mediaItems;
            ViewBag.MediaEntries = mediaEntries;
            ViewBag.CoinPackages = coinPackages;
            ViewBag.Transactions = transactions;
            ViewBag.EntryUnlocks = entryUnlocks;

            // counts for quick glance
            ViewBag.TotalUsers = users.Count;
            ViewBag.TotalMedia = mediaItems.Count;
            ViewBag.TotalEntries = mediaEntries.Count;
            ViewBag.TotalCoinPackages = coinPackages.Count;
            ViewBag.TotalTransactions = transactions.Count;
            ViewBag.TotalUnlocks = entryUnlocks.Count;

            return View();
        }

        public async Task<IActionResult> DashboardStats()
        {
            // User statistics
            var totalUsers = await _context.UserAccount.CountAsync();
            var totalAdmins = await _context.UserAccount.CountAsync(u => u.Role == "Admin");
            var totalActiveUsers = await _context.UserAccount.CountAsync(u => u.LastBonusClaimDate != null);

            // Media Item statistics
            var totalMedia = await _context.MediaItem.CountAsync();
            var animeCount = await _context.MediaItem.CountAsync(m => m.Type == "Anime");
            var mangaCount = await _context.MediaItem.CountAsync(m => m.Type == "Manga");
            var novelCount = await _context.MediaItem.CountAsync(m => m.Type == "Light Novel");

            // Media Entry statistics
            var totalEntries = await _context.MediaEntry.CountAsync();
            var totalUnlocks = await _context.EntryUnlocks.CountAsync();

            // Coin package statistics
            var totalCoinPackages = await _context.CoinPackages.CountAsync();

            // Transaction statistics
            var totalTransactions = await _context.Transactions.CountAsync();
            var totalCoinsAdded = await _context.Transactions.SumAsync(t => t.CoinsAdded);
            var averageTransactionAmount = totalTransactions > 0 ? await _context.Transactions.AverageAsync(t => t.Amount) : 0;


            // View data for display
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalActiveUsers = totalActiveUsers;
            ViewBag.TotalAdmins = totalAdmins;
            ViewBag.TotalMedia = totalMedia;
            ViewBag.AnimeCount = animeCount;
            ViewBag.MangaCount = mangaCount;
            ViewBag.NovelCount = novelCount;
            ViewBag.TotalEntries = totalEntries;
            ViewBag.TotalUnlocks = totalUnlocks;
            ViewBag.TotalCoinPackages = totalCoinPackages;
            ViewBag.TotalTransactions = totalTransactions;
            ViewBag.TotalCoinsAdded = totalCoinsAdded;
            ViewBag.AverageTransactionAmount = averageTransactionAmount;

            return View();
        }

        public async Task<IActionResult> TransactionHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch transactions for a specific user
            var transactions = await _context.Transactions
                .Where(t => t.UserId == id)
                .ToListAsync();

            if (transactions == null || !transactions.Any())
            {
                return NotFound();
            }

            return View(transactions);
        }
    }
}
