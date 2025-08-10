using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OtakuVault.Data;
using OtakuVault.Models;

namespace OtakuVault.Controllers
{
    public class CoinTransactionController : Controller
    {
        private readonly OtakuVaultContext _context;

        public CoinTransactionController(OtakuVaultContext context)
        {
            _context = context;
        }

        // GET: CoinTransaction
        public async Task<IActionResult> Index()
        {
            return View(await _context.CoinPackages.ToListAsync());
        }

        // GET: CoinTransaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coinPackage = await _context.CoinPackages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coinPackage == null)
            {
                return NotFound();
            }

            return View(coinPackage);
        }

        // GET: CoinTransaction/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CoinTransaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Coins,Price")] CoinPackage coinPackage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coinPackage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(coinPackage);
        }

        // GET: CoinTransaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coinPackage = await _context.CoinPackages.FindAsync(id);
            if (coinPackage == null)
            {
                return NotFound();
            }
            return View(coinPackage);
        }

        // POST: CoinTransaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Coins,Price")] CoinPackage coinPackage)
        {
            if (id != coinPackage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coinPackage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoinPackageExists(coinPackage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(coinPackage);
        }

        // GET: CoinTransaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coinPackage = await _context.CoinPackages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coinPackage == null)
            {
                return NotFound();
            }

            return View(coinPackage);
        }

        // POST: CoinTransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coinPackage = await _context.CoinPackages.FindAsync(id);
            if (coinPackage != null)
            {
                _context.CoinPackages.Remove(coinPackage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard", "Admin");
        }

        private bool CoinPackageExists(int id)
        {
            return _context.CoinPackages.Any(e => e.Id == id);
        }

        // GET: CoinTransaction/BuyCoins
        public IActionResult BuyCoins()
        {
            // Retrieve available packages for purchase
            var packages = _context.CoinPackages.ToList();
            return View(packages);
        }

        // POST: CoinTransaction/BuyCoins
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyCoins(int packageId, double customAmount)
        {
            // Ensure user is logged in
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            // Handle predefined package purchase
            if (packageId > 0)
            {
                var package = await _context.CoinPackages.FindAsync(packageId);
                if (package != null)
                {
                    // Process transaction and add coins
                    var user = await _context.UserAccount.FindAsync(userId);
                    if (user != null)
                    {
                        // Add coins to user's balance
                        user.OtakuVaultCoins += package.Coins;

                        // Create a transaction record
                        _context.Add(new Transaction
                        {
                            UserId = userId.Value,
                            Amount = package.Price,
                            CoinsAdded = package.Coins,
                            Date = DateTime.Now
                        });

                        await _context.SaveChangesAsync();

                        // Update session with new coin balance
                        HttpContext.Session.SetInt32("OtakuVaultCoins", user.OtakuVaultCoins);

                        return RedirectToAction(nameof(Success));
                    }
                }
            }

            // Handle custom coin purchase
            else if (customAmount > 0)
            {
                int coinsPurchased = (int)(customAmount * 10);  // 1 = 10 coins

                var user = await _context.UserAccount.FindAsync(userId);
                if (user != null)
                {
                    // Add coins to user's balance
                    user.OtakuVaultCoins += coinsPurchased;

                    // Create a transaction record
                    _context.Add(new Transaction
                    {
                        UserId = userId.Value,
                        Amount = customAmount,
                        CoinsAdded = coinsPurchased,
                        Date = DateTime.Now
                    });

                    await _context.SaveChangesAsync();

                    // Update session with new coin balance
                    HttpContext.Session.SetInt32("OtakuVaultCoins", user.OtakuVaultCoins);

                    return RedirectToAction(nameof(Success));
                }
            }

            // Handle failure or invalid state
            return View();
        }

        // GET: CoinTransaction/Success
        public IActionResult Success()
        {
            return View();
        }
    }
}
