using BusinessObject.Models;
using DataAccess.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.RoleRepositories
{
    public class RoleRepository : IRoleRepository
    {
        public List<Role> GetRoles()
        {
            return RoleDAO.GetRoles();
        }
    }
}
