namespace OtakuVault.Models
{
    public class UserMediaStatus
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int MediaID { get; set; }
        public required string Status { get; set; }
    }
}
