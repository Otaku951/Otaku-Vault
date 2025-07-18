namespace OtakuVault.Models
{
    public class GroupedMediaViewModel
    {
        public Dictionary<char, List<MediaItem>> GroupedMedia { get; set; } = new();
        public int ItemsPerGroupPage { get; set; } = 10;

        // Track current page per group: e.g., { 'A' => 2, 'B' => 1 }
        public Dictionary<char, int> GroupPageNumbers { get; set; } = new();
    }
}
