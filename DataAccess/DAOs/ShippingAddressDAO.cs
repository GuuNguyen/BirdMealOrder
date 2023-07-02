using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class ShippingAddressDAO
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

        public static ShippingAddress GetSAById(int id)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.ShippingAddresses.Find(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateSA(ShippingAddress shippingAddress)
        {
            try 
            {
                using(var context = new BirdMealOrderDBContext())
                {
                    context.ShippingAddresses.Add(shippingAddress);
                    context.SaveChanges();
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateSA(ShippingAddress shippingAddress)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    context.ShippingAddresses.Update(shippingAddress);
                    context.SaveChanges();
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Delete(int id)
        {
            try
            {
                using(var context = new BirdMealOrderDBContext())
                {
                    context.ShippingAddresses.Remove(context.ShippingAddresses.Find(id));
                    context.SaveChanges();
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
