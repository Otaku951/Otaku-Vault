namespace OtakuVault.Models
{
    public class MediaItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public int ReleaseYear { get; set; }
        public string? ExternalLink { get; set; }
    }
}
