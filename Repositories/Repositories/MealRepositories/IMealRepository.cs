﻿using BusinessObject.Models;
using Repositories.DTOs.MealDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.MealRepositories
{
    public interface IMealRepository
    {
        List<Meal> GetAllMeals();
        Meal GetMeal(int id);
        string CreateMeal(CreateMealDTO createMeal);
        void UpdateMeal(MealDTO mealDTO);
        void DeleteMeal(int id);
    }
}
