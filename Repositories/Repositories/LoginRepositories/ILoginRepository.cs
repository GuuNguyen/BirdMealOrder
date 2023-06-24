using BusinessObject.Models;
using Repositories.DTOs.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.LoginRepositories
{
    public interface ILoginRepository
    {
        User Login(AccountDTO account);
    }
}
