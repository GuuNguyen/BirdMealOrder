using BusinessObject.Models;
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
                using(var _context = new BirdMealOrderDBContext())
                {
                    _context.OrderDetails.Add(orderDetail);
                    _context.SaveChanges();
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
