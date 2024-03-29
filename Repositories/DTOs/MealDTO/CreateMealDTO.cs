﻿using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.MealDTO
{
    public class CreateMealDTO
    {
        public string MealName { get; set; } = null!;
        public string MealDescription { get; set; } = null!;
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public string? MealImage { get; set; }
        public List<ProductRequired> ProductOptions { get; set; }
        public List<int> BirdIds { get; set; }
    }

    public class ProductRequired
    {
        public int ProductId { get; set; }
        public int QuantityRequired { get; set; }
    }
}
