using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class MealDAO
    {
        public static Meal GetMeal(int id)
        {
            try
            {
                using(var _context = new BirdMealOrderDBContext())
                {
                    return _context.Meals.Find(id);
                }
            }catch(Exception ex)
            {
                throw  new Exception(ex.Message);
            }
        }
        public static void Update(Meal meal)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    _context.Meals.Update(meal);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
