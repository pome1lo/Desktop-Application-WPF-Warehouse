using System.ComponentModel.DataAnnotations;

namespace app.Models
{
    public class Statistics
    {
        [Key] public int Id { get; set; }
        public decimal Total { get; set; } = 0;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int NumberOfItemsSold { get; set; }
    }
}
