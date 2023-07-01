using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class ShippingAddressDTO
    {
        public static List<ShippingAddress> GetListSAByUserId(int userId)
        {
            try
            {
                using(var context = new BirdMealOrderDBContext())
                {
                    return context.ShippingAddresses.Where(s => s.UserId == userId).ToList();
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
