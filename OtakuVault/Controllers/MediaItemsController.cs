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
    public class MediaItemsController : Controller
    {
        private readonly OtakuVaultContext _context;

        public MediaItemsController(OtakuVaultContext context)
        {
            _context = context;
        }

        // GET: MediaItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.MediaItem.ToListAsync());
        }

        public async Task<IActionResult> Index1(string search, string type, string view = "card")
        {
            var media = _context.MediaItem.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                media = media.Where(m => m.Title.Contains(search));

            if (!string.IsNullOrEmpty(type) && type != "All")
                media = media.Where(m => m.Type == type);

            var mediaList = await media.ToListAsync();

            // Group by A–Z and store in ViewBag
            var grouped = mediaList
                .Where(m => !string.IsNullOrEmpty(m.Title))
                .GroupBy(m => char.ToUpper(m.Title[0]))
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.ToList());

            ViewBag.GroupedMedia = grouped;

            // Dropdown selections
            ViewBag.ViewType = view;
            ViewBag.Search = search;
            ViewBag.Type = type;

            ViewBag.TypeList = new SelectList(new[] { "All", "Anime", "Manga", "Novel" }, type);
            ViewBag.ViewList = new SelectList(new[]
            {
                new SelectListItem { Text = "Image View", Value = "card" },
                new SelectListItem { Text = "Text View", Value = "text" }
            }, "Value", "Text", view);

            return View(mediaList); // <- still pass full list to match @model
        }

        // GET: MediaItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaItem = await _context.MediaItem
                .FirstOrDefaultAsync(m => m.MediaID == id);
            if (mediaItem == null)
            {
                return NotFound();
            }

            return View(mediaItem);
        }


        // GET: MediaItems/Details/5
        public async Task<IActionResult> Details1(int? id)
        {
            if (id == null) return NotFound();

            var mediaItem = await _context.MediaItem
                .Include(m => m.Entries.OrderBy(e => e.Number))
                .FirstOrDefaultAsync(m => m.MediaID == id);
            if (mediaItem == null) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                ViewBag.UserStatus = null;
            }
            else
            {
                var status = await _context.UserMediaStatus
                    .FirstOrDefaultAsync(s => s.UserID == userId && s.MediaID == id);

                ViewBag.UserStatus = status?.Status;
            }

            ViewBag.StatusOptions = new List<string> { "Watching", "Completed", "Plan to Watch" };
            return View(mediaItem);
        }

        // POST: MediaItems/SaveStatus
        [HttpPost]
        public async Task<IActionResult> SaveStatus(int mediaId, string selectedStatus)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            var existing = await _context.UserMediaStatus
                .FirstOrDefaultAsync(s => s.UserID == userId && s.MediaID == mediaId);

            if (existing != null)
            {
                existing.Status = selectedStatus;
            }
            else
            {
                _context.UserMediaStatus.Add(new UserMediaStatus
                {
                    UserID = userId.Value,
                    MediaID = mediaId,
                    Status = selectedStatus
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = mediaId });
        }

        [HttpGet]
        public IActionResult CreateEntry(int mediaItemId)
        {
            var entry = new MediaEntry { MediaItemId = mediaItemId };
            return View(entry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEntry(MediaEntry entry)
        {
            if (ModelState.IsValid)
            {
                _context.MediaEntry.Add(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "MediaItems", new { id = entry.MediaItemId });
            }
            return View(entry);
        }

        // GET: MediaItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MediaItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MediaID,Title,Type,Genre,ImageData,ExternalLink")] MediaItem mediaItem, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(ms);
                    mediaItem.ImageData = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(mediaItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mediaItem);
        }

        // GET: MediaItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaItem = await _context.MediaItem.FindAsync(id);
            if (mediaItem == null)
            {
                return NotFound();
            }
            return View(mediaItem);
        }

        // POST: MediaItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MediaID,Title,Type,Genre,ImageData,ExternalLink")] MediaItem mediaItem, IFormFile? ImageFile)
        {
            var existingItem = await _context.MediaItem.FindAsync(id);
            if (existingItem == null) return NotFound();

            existingItem.Title = mediaItem.Title;
            existingItem.Type = mediaItem.Type;
            existingItem.Genre = mediaItem.Genre;
            existingItem.ExternalLink = mediaItem.ExternalLink;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await ImageFile.CopyToAsync(ms);
                existingItem.ImageData = ms.ToArray();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: MediaItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaItem = await _context.MediaItem
                .FirstOrDefaultAsync(m => m.MediaID == id);
            if (mediaItem == null)
            {
                return NotFound();
            }

            return View(mediaItem);
        }

        // POST: MediaItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mediaItem = await _context.MediaItem.FindAsync(id);
            if (mediaItem != null)
            {
                _context.MediaItem.Remove(mediaItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MediaItemExists(int id)
        {
            return _context.MediaItem.Any(e => e.MediaID == id);
        }
    }
}
