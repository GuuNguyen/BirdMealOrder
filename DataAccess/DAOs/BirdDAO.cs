using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class BirdDAO
    {
        public static List<Bird> GetAllBirds()
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.Birds.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
