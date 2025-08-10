using System.ComponentModel.DataAnnotations;

namespace OtakuVault.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }  
        public int CoinsAdded { get; set; } 
        public DateTime Date { get; set; }
    }
}
