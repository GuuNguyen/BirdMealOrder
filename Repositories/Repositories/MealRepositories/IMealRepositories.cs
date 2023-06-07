using BusinessObject.Models;
using Repositories.DTOs;
using Repositories.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.MealRepositories
{
    public interface IMealRepositories
    {
        List<Meal> GetAllMeals();
        Meal GetMeal(int id);
        void CreateMeal(CreateMeal createMeal);
        void UpdateMeal(MealDTO mealDTO);
        void DeleteMeal(int id);
    }
}
