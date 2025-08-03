namespace OtakuVault.Models
{
    public class MediaEntry
    {
        public int Id { get; set; }
        public int MediaItemId { get; set; }
        public string Title { get; set; }
        public int Release { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
