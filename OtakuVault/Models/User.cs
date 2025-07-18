using System.ComponentModel.DataAnnotations;

namespace OtakuVault.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        // Optional: Add Email, Role, etc.
        public string? Role { get; set; }
        public ICollection<UserMediaStatus>? UserMediaStatuses { get; set; }
    }
}
