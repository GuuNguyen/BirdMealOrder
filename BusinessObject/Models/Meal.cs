using BusinessObject.Enums;
using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Meal
    {
        public Meal()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int MealId { get; set; }
        public string? MealCode { get; set; }
        public string MealName { get; set; } = null!;
        public string MealDescription { get; set; } = null!;
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public string? MealImage { get; set; }
        public MealStatus MealStatus { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
