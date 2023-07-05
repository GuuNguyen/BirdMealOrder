using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class SearchDAO
    {
        public static object SearchAll(string keyWord)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var resultMeal = context.Meals.Where(m => m.MealName.ToLower().Contains(keyWord.ToLower())).ToList();
                    var resultProduct = context.Products.Where(p => p.ProductName.ToLower().Contains(keyWord.ToLower())).ToList();
                    return new
                    {
                        Meals = resultMeal,
                        Products = resultProduct,
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
