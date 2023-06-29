using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.UserDTO;
using Repositories.Repositories.UserRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userRepository.GetUsers());
        }

        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            return Ok(_userRepository.GetUser(userId));
        }

        [HttpPost]
        public IActionResult Create(CreateUserDTO userDTO)
        {
            var newUser = _userRepository.CreateUser(userDTO);
            return newUser ? Ok(newUser) : BadRequest();
        }


        [HttpPost("StaffCreateUser")]
        public IActionResult CreateFull(CreateUserDTOFull userDTOFull)
        {
            var newUser = _userRepository.CreateUserFull(userDTOFull);
            return newUser ? Ok(newUser) : BadRequest();
        }


        [HttpPut]
        public IActionResult Update(UpdateUserDTO userDTO)
        {
            var updateUser = _userRepository.UpdateUser(userDTO);
            return updateUser ? Ok(updateUser) : BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete(int userId)
        {
            var deleteUser = _userRepository.DeleteUser(userId);
            return deleteUser ? Ok("Delete success") : BadRequest();
        }

    }
}
