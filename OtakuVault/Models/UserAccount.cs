using System.ComponentModel.DataAnnotations;

namespace OtakuVault.Models
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        public required string Username { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public required string Role { get; set; }
    }
}
