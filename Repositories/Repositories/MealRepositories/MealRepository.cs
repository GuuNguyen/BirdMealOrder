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
    internal class MealRepository : IMealRepositories
    {
        public void CreateMeal(CreateMeal createMeal)
        {

        }

        public void DeleteMeal(int id)
        {
            throw new NotImplementedException();
        }

        public List<Meal> GetAllMeals()
        {
            throw new NotImplementedException();
        }

        public Meal GetMeal(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateMeal(MealDTO mealDTO)
        {
            throw new NotImplementedException();
        }
    }
}
