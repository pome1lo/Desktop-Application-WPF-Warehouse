using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{
    public class Order
    {
        [Key] public int Id { get; set; }
        public decimal Total { get; set; } = 0;
        public User User { get; set; } = null!;
        public List<ProductFromOrder> Products { get; set; } = new();
        public DateTime Date { get; set; } = DateTime.Now; 
    }
}
