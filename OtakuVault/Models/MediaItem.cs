using System.ComponentModel.DataAnnotations;

namespace OtakuVault.Models
{
    public class MediaItem
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Type { get; set; }
        public string? Genre { get; set; }
        public byte[]? ImageData { get; set; }
        public string? Tags { get; set; }
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? ExternalLink { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public ICollection<MediaEntry> Entries { get; set; } = new List<MediaEntry>(); // Link to episodes/chapters
    }
}
