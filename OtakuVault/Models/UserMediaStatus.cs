using System.ComponentModel.DataAnnotations;

namespace OtakuVault.Models
{
    public class UserMediaStatus
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public int MediaID { get; set; }
        public required string Status { get; set; }
        public UserAccount User { get; set; }
        public MediaItem Media { get; set; }
    }
}
