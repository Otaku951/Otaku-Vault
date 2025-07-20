using Microsoft.EntityFrameworkCore;
using OtakuVault.Data;
using OtakuVault.Models;
using System.Text;

namespace OtakuVault.Models
{
    public static class SeedData
    {
        public async static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new OtakuVaultContext(
                serviceProvider.GetRequiredService<DbContextOptions<OtakuVaultContext>>()))
            {
                if (context.MediaItem.Any())
                {
                    return; // DB has been seeded
                }

                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('MediaItem', RESEED, 0)");

                context.MediaItem.AddRange(
                    new MediaItem
                    {
                        Title = "Attack on Titan",
                        Type = "Anime",
                        Genre = "Action, Drama",
                        Tags = "Shonen, Titans, Survival",
                        Description = "Humans fight titans in a walled city.",
                        ReleaseDate = new DateTime(2013, 4, 7),
                        ExternalLink = "https://www.crunchyroll.com/attack-on-titan",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("FakeImageData1") // Replace with actual byte[]
                    },
                    new MediaItem
                    {
                        Title = "Solo Leveling",
                        Type = "Light Novel",
                        Genre = "Fantasy, Action",
                        Tags = "Hunter, Dungeon, Overpowered",
                        Description = "A weak hunter becomes the strongest through a leveling system.",
                        ReleaseDate = new DateTime(2018, 3, 4),
                        ExternalLink = "https://novelupdates.com/solo-leveling",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("FakeImageData2")
                    },
                    new MediaItem
                    {
                        Title = "Jujutsu Kaisen",
                        Type = "Manga",
                        Genre = "Action, Supernatural",
                        Tags = "Curses, Sorcery, School",
                        Description = "Yuji Itadori joins a secret school to fight curses.",
                        ReleaseDate = new DateTime(2018, 3, 5),
                        ExternalLink = "https://www.viz.com/jujutsu-kaisen",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("FakeImageData3")
                    },
                    new MediaItem
                    {
                        Title = "Violet Evergarden",
                        Type = "Anime",
                        Genre = "Drama, Fantasy",
                        Tags = "Emotional, Post-war, Slice of Life",
                        Description = "A former soldier writes letters to help others express emotions.",
                        ReleaseDate = new DateTime(2018, 1, 11),
                        ExternalLink = "https://www.netflix.com/title/80223227",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData4")
                    },
                    new MediaItem
                    {
                        Title = "Made in Abyss",
                        Type = "Anime",
                        Genre = "Adventure, Mystery",
                        Tags = "Exploration, Dark, Survival",
                        Description = "A girl and a robot descend into a deadly abyss to find her mother.",
                        ReleaseDate = new DateTime(2017, 7, 7),
                        ExternalLink = "https://www.hidive.com/tv/made-in-abyss",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData5")
                    },
                    new MediaItem
                    {
                        Title = "Mushoku Tensei",
                        Type = "Light Novel",
                        Genre = "Fantasy, Isekai",
                        Tags = "Reincarnation, Magic, Growth",
                        Description = "A jobless man reincarnates in a fantasy world with all memories intact.",
                        ReleaseDate = new DateTime(2014, 1, 23),
                        ExternalLink = "https://www.novelupdates.com/series/mushoku-tensei/",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData6")
                    },
                    new MediaItem
                    {
                        Title = "Berserk",
                        Type = "Manga",
                        Genre = "Dark Fantasy, Action",
                        Tags = "Medieval, Violence, Revenge",
                        Description = "Guts wields a massive sword seeking revenge in a dark fantasy world.",
                        ReleaseDate = new DateTime(1990, 8, 25),
                        ExternalLink = "https://www.viz.com/berserk",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData7")
                    },
                    new MediaItem
                    {
                        Title = "Re:Zero − Starting Life in Another World",
                        Type = "Light Novel",
                        Genre = "Fantasy, Psychological",
                        Tags = "Time Loop, Isekai, Romance",
                        Description = "Subaru gets stuck in a loop where death resets the day.",
                        ReleaseDate = new DateTime(2014, 1, 24),
                        ExternalLink = "https://www.novelupdates.com/series/rezero-kara-hajimeru-isekai-seikatsu/",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData8")
                    },
                    new MediaItem
                    {
                        Title = "Demon Slayer: Kimetsu no Yaiba",
                        Type = "Anime",
                        Genre = "Action, Supernatural",
                        Tags = "Demons, Swords, Shonen",
                        Description = "A boy becomes a demon slayer to avenge his family and save his sister.",
                        ReleaseDate = new DateTime(2019, 4, 6),
                        ExternalLink = "https://www.crunchyroll.com/series/GY5P48XEY/demon-slayer-kimetsu-no-yaiba",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData9")
                    },
                    new MediaItem
                    {
                        Title = "Steins;Gate",
                        Type = "Anime",
                        Genre = "Sci-Fi, Thriller",
                        Tags = "Time Travel, Science, Thriller",
                        Description = "A self-proclaimed mad scientist discovers a way to send messages to the past.",
                        ReleaseDate = new DateTime(2011, 4, 6),
                        ExternalLink = "https://www.crunchyroll.com/steinsgate",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData10")
                    },
                    new MediaItem
                    {
                        Title = "Death Note",
                        Type = "Anime",
                        Genre = "Psychological, Thriller",
                        Tags = "Mystery, Supernatural, Crime",
                        Description = "A high school student finds a notebook that can kill anyone whose name is written in it.",
                        ReleaseDate = new DateTime(2006, 10, 3),
                        ExternalLink = "https://www.netflix.com/title/70204970",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData11")
                    },
                    new MediaItem
                    {
                        Title = "Tokyo Ghoul",
                        Type = "Manga",
                        Genre = "Action, Horror",
                        Tags = "Ghouls, Identity, Dark",
                        Description = "Kaneki becomes a half-ghoul and struggles to survive in a world full of monsters.",
                        ReleaseDate = new DateTime(2011, 9, 8),
                        ExternalLink = "https://www.viz.com/tokyo-ghoul",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData12")
                    },
                    new MediaItem
                    {
                        Title = "Toradora!",
                        Type = "Anime",
                        Genre = "Romance, Comedy",
                        Tags = "School, Tsundere, Slice of Life",
                        Description = "A misunderstood delinquent teams up with a tiny tsundere to help each other with love.",
                        ReleaseDate = new DateTime(2008, 10, 2),
                        ExternalLink = "https://www.crunchyroll.com/toradora",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData13")
                    },
                    new MediaItem
                    {
                        Title = "Horimiya",
                        Type = "Manga",
                        Genre = "Romance, Slice of Life",
                        Tags = "High School, Secret Identity",
                        Description = "Two classmates with secret sides to themselves develop an unexpected relationship.",
                        ReleaseDate = new DateTime(2011, 10, 18),
                        ExternalLink = "https://www.viz.com/horimiya",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData14")
                    },
                    new MediaItem
                    {
                        Title = "Sword Art Online",
                        Type = "Light Novel",
                        Genre = "Action, Sci-Fi",
                        Tags = "VRMMO, Game, Adventure",
                        Description = "Players trapped in a VRMMO must fight to survive or die in real life.",
                        ReleaseDate = new DateTime(2009, 4, 10),
                        ExternalLink = "https://www.novelupdates.com/series/sword-art-online/",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData15")
                    },
                    new MediaItem
                    {
                        Title = "Spice and Wolf",
                        Type = "Light Novel",
                        Genre = "Adventure, Romance",
                        Tags = "Economy, Medieval, Fantasy",
                        Description = "A merchant and a wolf deity travel the countryside in a fantasy world full of trade and danger.",
                        ReleaseDate = new DateTime(2006, 2, 10),
                        ExternalLink = "https://www.novelupdates.com/series/spice-and-wolf/",
                        DateAdded = DateTime.Now,
                        ImageData = Encoding.UTF8.GetBytes("ImageData16")
                    }
                );

                context.SaveChanges();
            }
        }
    }
}