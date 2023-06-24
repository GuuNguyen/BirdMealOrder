using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.AccountDTO;
using Repositories.Repositories.LoginRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _repo;

        public LoginController(ILoginRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult Login(AccountDTO account)
        {
            var isLogin = _repo.Login(account);
            return isLogin != null ? Ok(isLogin) : BadRequest();
        }
    }
}
