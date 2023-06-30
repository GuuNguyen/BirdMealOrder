using BusinessObject.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class ProductDAO
    {
        public static List<Product> GetAllProducts()
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    return _context.Products.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<Product> GetProductsByMealId(int id)
        {
            try
            {
                using(var context = new BirdMealOrderDBContext())
                {
                    return context.MealProducts.Where(c => c.MealId == id).Select(p => p.Product).ToList();
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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
        public static Product GetProductByCode(string code)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    return _context.Products.Where(p => p.ProductCode == code).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Create(Product product)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    _context.Products.Add(product);
                    _context.SaveChanges();
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

        public static void Delete(int id)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    var p = _context.Products.Find(id);
                    _context.Remove(p);
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
