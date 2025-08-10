using System.ComponentModel.DataAnnotations;

namespace OtakuVault.Models
{
    public class EntryUnlock
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MediaEntryId { get; set; }
        public DateTime UnlockDate { get; set; }
    }
}
