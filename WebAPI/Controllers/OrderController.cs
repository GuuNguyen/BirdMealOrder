using BusinessObject.Enums;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult CreateOrder(CreateOrderDTO orderDTO)
        {
            var isSuccess = _repo.CreateOrder(orderDTO);
            return isSuccess ? Ok("Successfull created!") : BadRequest("Fail create!");
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateOrder(UpdateOrderDTO order)
        {
            var isSuccess = _repo.UpdateOrder(order);
            return isSuccess ? Ok("Successfull updated!") : BadRequest("Fail update!");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteOrder(int id)
        {
            var isSuccess = _repo.DeleteOrder(id);
            return isSuccess ? Ok("Successfull delete!") : BadRequest("Fail delete!");
        }

        [HttpGet("GetOrderByUserId/{userId}")]
        [Authorize]
        public IActionResult GetOrderByUserId(int userId)
        {
            return Ok(_repo.GetOrdersByUserId(userId));
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrder(int orderId)
        {
            return Ok(_repo.GetOrder(orderId));
        }

        [HttpGet("GetOrdersAndCheckHasReviewByUserId/{userId}")]
        [Authorize]
        public IActionResult GetOrdersAndCheckHasReviewByUserId(int userId)
        {
            return Ok(_repo.GetOrdersAndCheckHasReviewByUserId(userId));
        }

        [HttpPut("ChangeOrderStatus")]
        [Authorize]
        public IActionResult ChangeOrderStatus(ChangeOrderStatusDTO order)
        {
            var isSuccess = _repo.ChangeOrderStatus(order);
            return isSuccess ? Ok("Successfull updated!") : BadRequest("Fail update!");
        }

        [HttpPut("DeleteChangeOrderStatus/{id}")]
        [Authorize]
        public IActionResult DeleteChangeOrderStatus(int id)
        {
            var isSuccess = _repo.DeleteChangeOrderStatus(id);
            return isSuccess ? Ok("Successfull updated!") : BadRequest("Fail update!");
        }

        [HttpGet("GetBestSeller")]
        public IActionResult GetBestSeller()
        {
            return Ok(_repo.GetListBestSeller());
        }

        [HttpGet("RecommendList")]
        public IActionResult GetRecommendList()
        {
            return Ok(_repo.GetListRecommend());
        }
    }
}
