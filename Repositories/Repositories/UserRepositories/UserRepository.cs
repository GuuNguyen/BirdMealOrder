using AutoMapper;
using BusinessObject.Models;
using DataAccess.DAOs;
using Repositories.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;

        public UserRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public User GetUser(int id) => UserDAO.GetUser(id);

        public List<User> GetUsers() => UserDAO.GetUsers();

        public bool CreateUser(CreateUserDTO userDTO)
        {
            if(userDTO.FullName != null && userDTO.UserName != null && userDTO.PhoneNumber != null && userDTO.Password != null)
            {
                if(!UserDAO.isExitedUserName(userDTO.FullName) && !UserDAO.isExitedPhoneNumber(userDTO.PhoneNumber))
                {
                    var newUser = _mapper.Map<User>(userDTO);
                    newUser.RoleId = 2;
                    newUser.Status = 1;
                    UserDAO.Create(newUser);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateUser(UpdateUserDTO userDTO)
        {
            if (userDTO.FullName != null && userDTO.UserName != null && userDTO.PhoneNumber != null && userDTO.Password != null)
            {
                var updatedUser = _mapper.Map<User>(userDTO);
                UserDAO.Update(updatedUser);
                return true;
            }
            return false;
        }

        public bool DeleteUser(int id)
        {
            throw new NotImplementedException();
        }       
    }
}
