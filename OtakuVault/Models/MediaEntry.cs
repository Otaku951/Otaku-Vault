namespace OtakuVault.Models
{
    public class MediaEntry
    {
        public int Id { get; set; }
        public int MediaItemId { get; set; }

        public string Title { get; set; }
        public int Number { get; set; } // e.g., Episode 1, Chapter 1
        public DateTime ReleaseDate { get; set; }

        // Navigation property
        public MediaItem MediaItem { get; set; }
    }
}
