namespace OtakuVault.Models
{
    public class MediaEntry
    {
        public int Id { get; set; }
        public int MediaItemId { get; set; }
        public string Title { get; set; }
        public int Release { get; set; }
        public DateTime ReleaseDate { get; set; }

        public string ContentType { get; set; }

        // For Anime: store video data
        // For Manga: store image data 
        // For Novels: store text content
        public byte[]? ContentData { get; set; }
        public bool IsLocked { get; set; }
    }
}
