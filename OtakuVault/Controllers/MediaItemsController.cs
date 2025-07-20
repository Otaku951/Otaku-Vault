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

        // GET: MediaItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaItem = await _context.MediaItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mediaItem == null)
            {
                return NotFound();
            }

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
            return RedirectToAction("ReadingList", "User");
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
        public async Task<IActionResult> Create(MediaItem mediaItem, IFormFile ImageFile)
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
        public async Task<IActionResult> Edit(int id, MediaItem mediaItem, IFormFile ImageFile)
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
            /*
            if (id != mediaItem.Id)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await imageFile.CopyToAsync(ms);
                    mediaItem.ImageData = ms.ToArray();
                }
            }
            else
            {
                // retain existing image data
                var existingItem = await _context.MediaItem.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                mediaItem.ImageData = existingItem?.ImageData;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mediaItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MediaItemExists(mediaItem.Id))
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
            return View(mediaItem);*/
        }

        // GET: MediaItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaItem = await _context.MediaItem
                .FirstOrDefaultAsync(m => m.Id == id);
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
            return _context.MediaItem.Any(e => e.Id == id);
        }
    }
}
