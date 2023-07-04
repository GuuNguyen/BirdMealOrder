using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        public decimal UnitPrice { get; set; }

        public virtual Meal? Meal { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; } = null!;
        public virtual Product? Product { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
