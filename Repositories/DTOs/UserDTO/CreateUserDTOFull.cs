using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.UserDTO
{
    public class CreateUserDTOFull
    {
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int RoleId { get; set; }
    }
}
