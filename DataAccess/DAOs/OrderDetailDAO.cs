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
    }
}
