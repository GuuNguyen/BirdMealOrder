using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class RoleDAO
    {
        public static List<Role> GetRoles()
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.Roles.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
