using BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.MealDTO
{
    public class UpdateMealDTO
    {
        [Required]
        public int MealId { get; set; }
        [Required]
        public string? MealCode { get; set; }
        [Required]
        public string MealName { get; set; }
        [Required]
        public string MealDescription { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int QuantityAvailable { get; set; }
        [Required]
        public string MealImage { get; set; }
        [Required]
        public MealStatus MealStatus { get; set; }
        [Required]
        public List<ProductRequired> ProductRequireds { get; set; }
        [Required]
        public List<int> BirdIds { get; set; }
    }
}
