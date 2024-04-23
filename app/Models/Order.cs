using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{
    public class Order
    {
        public int Id { get; set; } = default;
        public decimal Total { get; set; } = 0;
        public User User { get; set; } = null!;
        public DateTime Date { get; set; } = DateTime.Now; 
    }
}
