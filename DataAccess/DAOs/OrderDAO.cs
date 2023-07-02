using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class OrderDAO
    {
        public static List<Order> GetOrders()
        {
            try
            {
                using(var _context = new BirdMealOrderDBContext())
                {
                    return _context.Orders.ToList();
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }         
        }

        public static Order GetOrder(int id)
        {
            try
            {
                using(var _context = new BirdMealOrderDBContext())
                {
                    return _context.Orders.Find(id);
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateOrder(Order order)
        {
            try
            {
                using(var _context = new BirdMealOrderDBContext())
                {
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Update(Order order)
        {
            try
            {
                using(var _context = new BirdMealOrderDBContext())
                {
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteOrder(int id)
        {
            try
            {
                using(var _context = new BirdMealOrderDBContext())
                {
                    var order = _context.Orders.Find(id);
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
