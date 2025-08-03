using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> BuyCoins(int packageId, decimal customAmount)
        {
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
                    if (user != null && user.Balance >= package.Price)
                    {
                        user.Balance -= package.Price;
                        user.OtakuVaultCoins += package.Coins;

                        _context.Add(new Transaction
                        {
                            UserId = userId.Value,
                            Amount = package.Price,
                            CoinsAdded = package.Coins,
                            Date = DateTime.Now
                        });

                        await _context.SaveChangesAsync();

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
                if (user != null && user.Balance >= customAmount)
                {
                    user.Balance -= customAmount;
                    user.OtakuVaultCoins += coinsPurchased;

                    _context.Add(new Transaction
                    {
                        UserId = userId.Value,
                        Amount = customAmount,
                        CoinsAdded = coinsPurchased,
                        Date = DateTime.Now
                    });

                    await _context.SaveChangesAsync();

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
