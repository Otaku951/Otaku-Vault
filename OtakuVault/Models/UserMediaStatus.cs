using System.ComponentModel.DataAnnotations;

namespace OtakuVault.Models
{
    public class UserMediaStatus
    {
        [Key]
        public int StatusID { get; set; }
        public int UserID { get; set; }
        public int MediaID { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
        public MediaItem Media { get; set; }
    }
}
