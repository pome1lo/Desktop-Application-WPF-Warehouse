using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{
    public class Product
    {
        public int Id { get; set; } = default;
        public string ProductName { get; set; } = string.Empty;
        public string Desciption { get; set; } = string.Empty;
        public Category Category { get; set; } = null!;
        public decimal Price{ get; set; } = 0;
        public string Image { get; set; } = @"\Static\Img\ImgDefault.png";
    }
}
