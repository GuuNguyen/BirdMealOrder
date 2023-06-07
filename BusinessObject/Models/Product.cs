using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public string? ProductImage { get; set; }
        public int ProductStatus { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
