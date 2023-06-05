using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class BirdMeal
    {
        public int BirdId { get; set; }
        public int MealId { get; set; }

        public virtual Bird Bird { get; set; } = null!;
        public virtual Meal Meal { get; set; } = null!;
    }
}
