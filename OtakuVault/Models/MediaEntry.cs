namespace OtakuVault.Models
{
    public class MediaEntry
    {
        public int Id { get; set; }
        public int MediaItemId { get; set; }

        public string Group { get; set; }
        public int Release { get; set; } // Episode 1, Chapter 1
        public DateTime ReleaseDate { get; set; }
    }
}
