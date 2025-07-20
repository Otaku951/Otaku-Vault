using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OtakuVault.Data;

namespace OtakuVault.Controllers
{
    public class MediaLibraryController : Controller
    {
        private readonly OtakuVaultContext _context;

        public MediaLibraryController(OtakuVaultContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, string type, string view = "card")
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

            return View(mediaList);
        }

        // GET: MediaItems/Details/5
        public async Task<IActionResult> MediaDetails(int? id)
        {
            if (id == null) return NotFound();

            var mediaItem = await _context.MediaItem
                .Include(m => m.Entries.OrderBy(e => e.Release))
                .FirstOrDefaultAsync(m => m.Id == id);
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
    }
}
