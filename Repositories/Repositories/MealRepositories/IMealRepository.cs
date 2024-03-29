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
        List<Meal> GetMealsByIds(List<int> mealIds);
        List<Meal> GetMealsByBirdId(int birdId);
        Meal GetMealByCode(string code);
        string CreateMeal(CreateMealDTO createMeal);
        string UpdateMeal(UpdateMealDTO mealDTO);
        void DeleteMeal(int id);
        void ChangeStatus(int mealId);

        UpdateMealDTO GetMealInclueBirdAndProduct(int mealId);

        List<MealProduct> GetMealProductsByMealId(int mealId);
    }
}
