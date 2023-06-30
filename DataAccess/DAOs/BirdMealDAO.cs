using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class BirdMealDAO
    {
        public static void Create(BirdMeal birdMeal)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var query = $"INSERT INTO BirdMeal (BirdId, MealId) VALUES ({birdMeal.BirdId}, {birdMeal.MealId});";
                    context.Database.ExecuteSqlRaw(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateRange(List<BirdMeal> birdMeals)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    foreach (var birdMeal in birdMeals)
                    {
                        var query = $"INSERT INTO BirdMeal (BirdId, MealId) VALUES ({birdMeal.BirdId}, {birdMeal.MealId});";
                        context.Database.ExecuteSqlRaw(query);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteByMealId(int mealId)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var query = $"DELETE FROM BirdMeal WHERE MealId= {mealId};";
                    context.Database.ExecuteSqlRaw(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
