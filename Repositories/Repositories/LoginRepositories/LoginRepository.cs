using BusinessObject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.DTOs.AccountDTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.LoginRepositories
{
    public class LoginRepository : ILoginRepository
    {
        public readonly BirdMealOrderDBContext _context;
        public readonly IConfiguration _config;

        public LoginRepository(BirdMealOrderDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public object Login(AccountDTO account)
        {
            if (account.UserName != null && account.Password != null)
            {
                var roleString = "";
                var user = _context.Users.Where(u => u.UserName == account.UserName && u.Password == account.Password).FirstOrDefault();
                if (user != null)
                {
                    var role = _context.Roles.Where(r => r.RoleId == user.RoleId).FirstOrDefault();
                    if (role != null)
                    {
                        roleString = role.RoleName;
                    }
                    var claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                        new Claim(ClaimTypes.Role, roleString),
                    };

                    var token = new JwtSecurityToken(
                        issuer: _config["JWT:Issuer"],
                        audience: _config["JWT:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"])),
                        SecurityAlgorithms.HmacSha256)
                    );
                    return new { token = new JwtSecurityTokenHandler().WriteToken(token) };
                }
            }
            return null;
        }
    }
}
