using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class GenerateCodeDAO
    {
        private static Random random = new Random();

        public static string GenerateProductCode(string productType)
        {

            string prefix = GetPrefix(productType);
            string randomDigits = GenerateRandomDigits(6);
            string productCode = prefix + randomDigits;
            var listStr = GetCode(productType);
            while (listStr.Contains(productCode))
            {
                randomDigits = GenerateRandomDigits(6);
                productCode = prefix + randomDigits;
            }
            return productCode;
        }
        public static List<string> GetCode(string productType)
        {
            dynamic product, meal;
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    switch (productType)
                    {
                        case "Meal":
                            return meal = context.Meals.Select(m => m.MealCode).ToList();
                        case "Product":
                            return product = context.Products.Select(p => p.ProductCode).ToList();
                        default:
                            throw new ArgumentException("Invalid product type.");
                    }
                }
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }    
        }
        public static string GetPrefix(string productType)
        {
            switch (productType)
            {
                case "Meal":
                    return "ME";
                case "Product":
                    return "FO";
                default:
                    throw new ArgumentException("Invalid product type.");
            }
        }

        public static string GenerateRandomDigits(int length)
        {
            string digits = random.Next(0, (int)Math.Pow(10, length)).ToString("D" + length);
            return digits;
        }
    }

}
