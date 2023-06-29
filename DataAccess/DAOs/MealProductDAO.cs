using Azure.Core;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class MealProductDAO
    {
        public static void Create(MealProduct mealProduct)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var query = $"INSERT INTO MealProduct (ProductId, MealId, QuantityRequired) VALUES ({mealProduct.ProductId}, {mealProduct.MealId}, {mealProduct.QuantityRequired});";
                    context.Database.ExecuteSqlRaw(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateRange(List<MealProduct> mealProducts, int quantityMeal)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    foreach (var mealProduct in mealProducts)
                    {
                        var query = $"INSERT INTO MealProduct (ProductId, MealId, QuantityRequired) VALUES ({mealProduct.ProductId}, {mealProduct.MealId}, {mealProduct.QuantityRequired});";
                        context.Database.ExecuteSqlRaw(query);
                        var product = context.Products.Find(mealProduct.ProductId);
                        product.QuantityAvailable = product.QuantityAvailable - mealProduct.QuantityRequired * quantityMeal;
                        context.Update(product);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string CheckQuantityProductAvailable(int productId, int quantityRequired)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext()) // Thay YourDbContext bằng tên DbContext của ứng dụng của bạn
                {

                    var product = context.Products.Find(productId);

                    if (quantityRequired > product.QuantityAvailable)
                    {
                        return $"The quantity required of '{product.ProductName}' is greater than the available quantity({product.QuantityAvailable})!";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<MealProduct> GetProductsByMealId(int mealId)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.MealProducts.Where(mp => mp.MealId == mealId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
