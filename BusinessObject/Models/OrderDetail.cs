using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            Feedbacks = new HashSet<Feedback>();
        }

        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? MealId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public virtual Meal? Meal { get; set; }
        public virtual Order Order { get; set; } = null!;
        public virtual Product? Product { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
