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
    public class MediaEntriesController : Controller
    {
        private readonly OtakuVaultContext _context;

        public MediaEntriesController(OtakuVaultContext context)
        {
            _context = context;
        }

        // GET: MediaEntries
        public async Task<IActionResult> Index()
        {
            return View(await _context.MediaEntry.ToListAsync());
        }

        // GET: MediaItems/ViewEntry/5
        public IActionResult ViewEntry(int? id)
        {
            var entry = _context.MediaEntry.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound();
            }
            // Get the previous and next entries for navigation
            var previousEntry = _context.MediaEntry
                .Where(e => e.MediaItemId == entry.MediaItemId && e.Release < entry.Release)
                .OrderByDescending(e => e.Release)
                .FirstOrDefault();

            var nextEntry = _context.MediaEntry
                .Where(e => e.MediaItemId == entry.MediaItemId && e.Release > entry.Release)
                .OrderBy(e => e.Release)
                .FirstOrDefault();

            ViewBag.PreviousEntry = previousEntry;
            ViewBag.NextEntry = nextEntry;
            return View(entry);
        }

        [HttpPost]
        public async Task<IActionResult> UnlockEntry(int entryId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            var entry = await _context.MediaEntry.FindAsync(entryId);
            if (entry == null || !entry.IsLocked) return NotFound();

            var user = await _context.UserAccount.FindAsync(userId);

            // Check if the user has already unlocked the entry
            bool alreadyUnlocked = await _context.EntryUnlocks
                .AnyAsync(u => u.UserId == userId && u.MediaEntryId == entryId);
            if (alreadyUnlocked) return RedirectToAction("ViewEntry", new { id = entryId });

            // Check if the user has enough coins to unlock the entry
            int unlockCost = 50; 
            if (user.OtakuVaultCoins < unlockCost)
            {
                return BadRequest("Not enough coins");
            }

            // Deduct coins and mark entry as unlocked
            user.OtakuVaultCoins -= unlockCost;
            HttpContext.Session.SetInt32("OtakuVaultCoins", user.OtakuVaultCoins);

            // Record the unlock transaction
            _context.EntryUnlocks.Add(new EntryUnlock
            {
                UserId = userId.Value,
                MediaEntryId = entryId,
                UnlockDate = DateTime.Now
            });

            await _context.SaveChangesAsync();

            return RedirectToAction("ViewEntry", new { id = entryId });
        }


        // GET: MediaEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaEntry = await _context.MediaEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mediaEntry == null)
            {
                return NotFound();
            }

            return View(mediaEntry);
        }

        // GET: MediaEntries/Create
        public IActionResult Create(int mediaId)
        {
            var entry = new MediaEntry { MediaItemId = mediaId };

            return View(entry);
        }

        // POST: MediaEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MediaEntry mediaEntry, IFormFile contentFile)
        {
            // Handle the content upload based on type (Video, Image, or Text)
            if (contentFile != null && contentFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await contentFile.CopyToAsync(ms);
                    mediaEntry.ContentData = ms.ToArray();  // Store file content as byte array
                }
            }

            mediaEntry.ReleaseDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                // Lock the latest entry when a new entry is added
                var latestEntry = await _context.MediaEntry
                    .Where(e => e.MediaItemId == mediaEntry.MediaItemId)
                    .OrderByDescending(e => e.ReleaseDate)
                    .FirstOrDefaultAsync();

                if (latestEntry != null)
                {
                    // Unlock the previous latest entry
                    latestEntry.IsLocked = false; 
                    _context.Update(latestEntry);
                }
                
                // Lock the new latest entry
                mediaEntry.IsLocked = true; 

                _context.Add(mediaEntry);
                await _context.SaveChangesAsync();

                return RedirectToAction("MediaDetails", "MediaLibrary", new { id = mediaEntry.MediaItemId });
            }
            return View(mediaEntry);
        }

        // GET: MediaEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaEntry = await _context.MediaEntry.FindAsync(id);
            if (mediaEntry == null)
            {
                return NotFound();
            }
            return View(mediaEntry);
        }

        // POST: MediaEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MediaEntry mediaEntry, IFormFile contentFile)
        {
            if (id != mediaEntry.Id)
            {
                return NotFound();
            }

            // Check if the entry exists
            var existingEntry = await _context.MediaEntry.FindAsync(id);
            if (existingEntry == null)
            {
                return NotFound();
            }

            // Update entry fields
            existingEntry.Title = mediaEntry.Title;
            existingEntry.Release = mediaEntry.Release;
            existingEntry.ReleaseDate = mediaEntry.ReleaseDate;
            existingEntry.ContentType = mediaEntry.ContentType;

            // Handle file upload (content)
            if (contentFile != null && contentFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await contentFile.CopyToAsync(ms);
                existingEntry.ContentData = ms.ToArray();  // Update content data with the new file
            }

            // Update the entry in the database
            _context.Update(existingEntry);
            await _context.SaveChangesAsync();

            return RedirectToAction("MediaDetails", "MediaLibrary", new { id = existingEntry.MediaItemId });
        }

        // GET: MediaEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaEntry = await _context.MediaEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mediaEntry == null)
            {
                return NotFound();
            }

            return View(mediaEntry);
        }

        // POST: MediaEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mediaEntry = await _context.MediaEntry.FindAsync(id);
            if (mediaEntry != null)
            {
                _context.MediaEntry.Remove(mediaEntry);
            }

            await _context.SaveChangesAsync();

            // After deletion, lock the latest entry for the same media item
            var latestEntry = await _context.MediaEntry
                    .Where(e => e.MediaItemId == mediaEntry.MediaItemId)
                    .OrderByDescending(e => e.ReleaseDate)
                    .FirstOrDefaultAsync();

            if (latestEntry != null)
            {
                latestEntry.IsLocked = true; 
                _context.Update(latestEntry);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("MediaDetails", "MediaLibrary", new { id = mediaEntry.MediaItemId });
        }

        private bool MediaEntryExists(int id)
        {
            return _context.MediaEntry.Any(e => e.Id == id);
        }
    }
}
