using System.ComponentModel.DataAnnotations;

namespace OtakuVault.Models
{
    public class MediaItem
    {
        [Key]
        public int MediaID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Type { get; set; } // Anime, Manga, Novel

        public string? Genre { get; set; }

        public byte[]? ImageData { get; set; }

        public string? ExternalLink { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime? ReleaseDate { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        public ICollection<MediaEntry> Entries { get; set; } // Link to episodes/chapters
        public ICollection<UserMediaStatus> UserMediaStatuses { get; set; }
    }
}
