using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModel
{
    public class CreateMeal
    {
        public string MealName { get; set; } = null!;
        public string MealDescription { get; set; } = null!;
        public double Price { get; set; }
        public string? MealImage { get; set; }
        public int MealStatus { get; set; }
    }
}
