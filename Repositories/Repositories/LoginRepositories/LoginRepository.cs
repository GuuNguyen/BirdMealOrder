using BusinessObject.Models;
using Repositories.DTOs.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.LoginRepositories
{
    public class LoginRepository : ILoginRepository
    {
        public readonly BirdMealOrderDBContext _context;

        public LoginRepository(BirdMealOrderDBContext context)
        {
            _context = context;
        }

        public User Login(AccountDTO account)
        {
            if(account.UserName != null && account.Password != null)
            {
                var user = _context.Users.Where(u => u.UserName == account.UserName && u.Password == account.Password).FirstOrDefault();
                if(user != null)
                {
                    return user;
                }
            }
            return null;
        }
    }
}
