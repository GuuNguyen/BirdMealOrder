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
                using (var _context = new BirdMealOrderDBContext())
                {
                    return _context.Orders.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Order GetOrder(int id)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    return _context.Orders.Find(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateOrder(Order order)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Update(Order order)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteOrder(int id)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    var order = _context.Orders.Find(id);
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<Order> GetOrdersByUserId(int userId)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    return _context.Orders.Where(o => o.UserId == userId).Include(o => o.ShippingAddress).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<object> GetOrdersAndCheckHasReviewByUserId(int userId)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var ordersInfo = context.Orders
                        .Where(order => order.UserId == userId)
                        .Include(order => order.OrderDetails)
                            .ThenInclude(orderDetail => orderDetail.Meal)
                        .Include(order => order.OrderDetails)
                            .ThenInclude(orderDetail => orderDetail.Product)
                        .Select(order => new
                        {
                            Order = order,
                            AllReview = order.OrderDetails.All(orderDetail => orderDetail.Feedbacks.Any())
                        })
                        .ToList<object>();

                    return ordersInfo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
