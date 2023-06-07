using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class ProductDAO
    {
        public static Product GetProductById(int id)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    return _context.Products.Find(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateProduct(Product product)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    _context.Products.Update(product);
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
