namespace OtakuVault.Models
{
    public class UserAccount
    {
        public int Id { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }

        public required string Role { get; set; }
    }
}
