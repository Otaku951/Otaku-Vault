using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OtakuVault.Data;
using OtakuVault.Migrations;
using OtakuVault.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OtakuVault.Controllers
{
    public class AccountController : Controller
    {
        private readonly OtakuVaultContext _context;

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public AccountController(OtakuVaultContext context)
        {
            _context = context;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = _context.User.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserID", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(User newUser)
        {
            newUser.Role = "User";
            if (ModelState.IsValid)
            {
                ViewBag.Debug = "Form received and ModelState is valid.";
                // Check if username is already taken
                if (_context.User.Any(u => u.Username == newUser.Username))
                {
                    ModelState.AddModelError("Username", "Username is already in use.");
                    
                    return View(newUser);
                }
                // Hash the password before saving
                newUser.Password = HashPassword(newUser.Password);

                // Set default role
                
                _context.User.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            ViewBag.Debug = "Form received but ModelState is invalid.";
            if (!ModelState.IsValid)
            {
                // Print all model validation errors to the console
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        ViewBag.Debug = $"[Model Error] {state.Key}: {error.ErrorMessage}";
                    }
                }

                return View(newUser);
            }
            return View(newUser);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
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

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Index", "Home");

            var users = _context.User.ToList();
            var media = _context.MediaItem.ToList();

            ViewBag.Users = users;
            ViewBag.MediaItems = media;

            return View();
        }

        public async Task<IActionResult> DashboardStats()
        {
            var totalUsers = await _context.User.CountAsync();
            var totalAdmins = await _context.User.CountAsync(u => u.Role == "Admin");
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
                    MediaTitle = _context.MediaItem.FirstOrDefault(m => m.MediaID == g.Key).Title,
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

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.Password = HashPassword(user.Password); // Ensure password stays hashed
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
