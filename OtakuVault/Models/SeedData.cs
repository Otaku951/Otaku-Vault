using Microsoft.EntityFrameworkCore;
using OtakuVault.Data;
using System.Reflection.Emit;

namespace OtakuVault.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new OtakuVaultContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<OtakuVaultContext>>()))
            {
                // Look for any movies.
                if (context.MediaItem.Any())
                {
                    return;   // DB has been seeded
                }
                context.MediaItem.AddRange(
                    new MediaItem
                    {
                        Title = "Attack on Titan",
                        Type = "Anime",
                        Genre = "Action, Drama, Fantasy",
                        ExternalLink = "https://anilist.co/anime/16498",
                        DateAdded = DateTime.Now,
                        ReleaseDate = new DateTime(2013, 4, 6),
                        Description = "Humanity fights for survival against titans.",
                        Tags = "Titans, War, Walls"
                    },
                    new MediaItem
                    {
                        Title = "Solo Leveling",
                        Type = "Manga",
                        Genre = "Action, Fantasy",
                        ExternalLink = "https://novelupdates.com/series/solo-leveling/",
                        DateAdded = DateTime.Now,
                        ReleaseDate = new DateTime(2018, 3, 4),
                        Description = "A weak hunter rises to become the strongest.",
                        Tags = "Dungeon, Gates, System"
                    }
                );
                context.SaveChanges();
                // Look for any movies.
                if (context.MediaEntry.Any())
                {
                    return;   // DB has been seeded
                }
                // Seed MediaEntry
                context.MediaEntry.AddRange(
                     new MediaEntry
                     {
                         MediaItemId = 20,
                         Number = 1,
                         Title = "To You, in 2000 Years",
                         ReleaseDate = new DateTime(2013, 4, 6)
                     },
                    new MediaEntry
                    {
                        MediaItemId = 21,
                        Number = 2,
                        Title = "That Day",
                        ReleaseDate = new DateTime(2013, 4, 13)
                    }
                );
            }
        }
    }
}
