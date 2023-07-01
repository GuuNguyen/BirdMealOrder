using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.MealDTO
{
    public class CartItemMealDTO
    {
        public int MealId { get; set; }
        public string? MealCode { get; set; }
        public string MealName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? MealImage { get; set; }
        public int Quantity { get; set; }
    }
}
