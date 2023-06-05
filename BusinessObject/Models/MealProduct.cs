using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MealProduct
    {
        public int ProductId { get; set; }
        public int MealId { get; set; }
        public int QuantityRequired { get; set; }

        public virtual Meal Meal { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
