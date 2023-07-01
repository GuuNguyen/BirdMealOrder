using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.ProductDTO
{
    public class CartItemProductDTO
    {
        public int ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ProductImage { get; set; }
        public int Quantity { get; set; }
    }
}
