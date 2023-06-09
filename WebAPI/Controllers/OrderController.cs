using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.OrderDTO;
using Repositories.Repositories.OrderRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repo;
        public OrderController(IOrderRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetOrders());   
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderDTO orderDTO)
        {
            var isSuccess = _repo.CreateOrder(orderDTO);
            return isSuccess ? Ok("Successfull created!") : BadRequest("Fail create!");
        }

        [HttpPut]
        public IActionResult UpdateOrder(UpdateOrderDTO order)
        {
            var isSuccess = _repo.UpdateOrder(order);
            return isSuccess ? Ok("Successfull updated!") : BadRequest("Fail update!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var isSuccess = _repo.DeleteOrder(id);
            return isSuccess ? Ok("Successfull delete!") : BadRequest("Fail delete!");
        }
    }
}
