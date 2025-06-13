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
    public class MediaController : Controller
    {
        private readonly OtakuVaultContext _context;

        public MediaController(OtakuVaultContext context)
        {
            _context = context;
        }

        // GET: Media
        public async Task<IActionResult> Index()
        {
            var mediaList = new List<MediaItem>
            {
                new MediaItem { Id = 1, Title = "One Piece", Type = "Anime", Genre = "Adventure", ExternalLink = "https://www.crunchyroll.com/series/GYQ4MW246/one-piece" },
                new MediaItem { Id = 2, Title = "Solo Leveling", Type = "Manga", Genre = "Action", ExternalLink = "https://www.webtoons.com/en/action/solo-leveling/list?title_no=3182" },
                new MediaItem { Id = 3, Title = "Re:Zero", Type = "Novel", Genre = "Fantasy", ExternalLink = "https://www.novelupdates.com/series/rezero-kara-hajimeru-isekai-seikatsu/" }
            };

            return View(mediaList);
        }

        // GET: Media/Details/5
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

        // GET: Media/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Media/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Type,Genre,Description,ReleaseYear,ExternalLink")] MediaItem mediaItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mediaItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mediaItem);
        }

        // GET: Media/Edit/5
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

        // POST: Media/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Type,Genre,Description,ReleaseYear,ExternalLink")] MediaItem mediaItem)
        {
            if (id != mediaItem.Id)
            {
                return NotFound();
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
            return View(mediaItem);
        }

        // GET: Media/Delete/5
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

        // POST: Media/Delete/5
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
