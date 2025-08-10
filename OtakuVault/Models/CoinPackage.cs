using System.ComponentModel.DataAnnotations;

namespace OtakuVault.Models
{
    public class CoinPackage
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } 
        public int Coins { get; set; } 
        public double Price { get; set; } 
    }
}
