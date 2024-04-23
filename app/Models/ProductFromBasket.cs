using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{
    public class ProductFromBasket
    {
        public int Id { get; set; } = default;
        public Product Product { get; set; } = null!;
        public ushort Quantity { get; set; } = 1;
    }
}
