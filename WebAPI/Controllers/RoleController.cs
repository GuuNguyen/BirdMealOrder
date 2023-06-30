using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories.RoleRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleRepository roleRepository = new RoleRepository();
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(roleRepository.GetRoles());
        }
    }
}
