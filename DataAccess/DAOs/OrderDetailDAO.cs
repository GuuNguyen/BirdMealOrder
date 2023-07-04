using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class OrderDetailDAO
    {
        public static void CreateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using (var _context = new BirdMealOrderDBContext())
                {
                    _context.OrderDetails.Add(orderDetail);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool CheckMealInOderDetails(int mealId)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var meal = context.OrderDetails.FirstOrDefault(o => o.MealId == mealId);
                    if (meal == null) return false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static OrderDetail GetOrderDetail(int id)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.OrderDetails.FirstOrDefault(o => o.OrderDetailId == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<object> GetOrderDetails()
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.OrderDetails.Include(o => o.Meal).Include(o => o.Product)
                                    .Join(context.Orders, od => od.OrderId, o => o.OrderId, (od, o) => new { OrderDetail = od, Order = o })
                                    .Join(context.Users, o => o.Order.UserId, u => u.UserId, (o, u) => new { OrderDetail = o.OrderDetail, Order = o.Order, User = u })
                                    .Select(result => new
                                    {
                                        OrderDetail = result.OrderDetail,
                                        UserName = result.User.UserName,
                                        Order = result.Order,
                                    }).ToList<object>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
