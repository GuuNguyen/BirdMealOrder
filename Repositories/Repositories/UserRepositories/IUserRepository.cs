using BusinessObject.Models;
using Repositories.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.UserRepositories
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUser(int id);
        bool CreateUser(CreateUserDTO userDTO);
        bool UpdateUser(UpdateUserDTO userDTO);
        bool DeleteUser(int id);    
    }
}
