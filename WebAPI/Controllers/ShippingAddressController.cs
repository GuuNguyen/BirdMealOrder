using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.ShippingAddressDTO;
using Repositories.Repositories.ShippingAddressRepositories;

namespace WebAPI.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IShippingAddressRepository _repo;

        public ShippingAddressController(IShippingAddressRepository repo)
        {
            _repo = repo;
        }

        
        [HttpGet("ListBy/{userId}")]
        public IActionResult GetListByUserId(int userId)
        {
            return Ok(_repo.GetAllByUserId(userId));
        }

        [HttpGet("{id}")]
        public IActionResult GetListById(int id)
        {
            return Ok(_repo.GetById(id));   
        }


        [HttpPost]
        public IActionResult Create(CreateSADTO createSADTO)
        {
            var isSuccess = _repo.CreateSA(createSADTO);
            return isSuccess ? Ok(isSuccess) : BadRequest(isSuccess);
        }

        [HttpPut]
        public IActionResult Update(UpdateSADTO updateSADTO)
        {
            var isSuccess = _repo.UpdateSA(updateSADTO);
            return isSuccess ? Ok(isSuccess) : BadRequest(isSuccess);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isSuccess = _repo.DeleteSA(id);
            return isSuccess ? Ok(isSuccess) : BadRequest(isSuccess);
        }
    }
}
