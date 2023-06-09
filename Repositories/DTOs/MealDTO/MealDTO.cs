using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.MealDTO
{
    public class MealDTO
    {
        public int MealId { get; set; }
        public string MealName { get; set; } = null!;
        public string MealDescription { get; set; } = null!;
        public double Price { get; set; }
        public string? MealImage { get; set; }
        public MealStatus MealStatus { get; set; }
    }
}
