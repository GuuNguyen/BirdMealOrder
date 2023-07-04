using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories.OrderDetailRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private IOrderDetailRepository _orderDetailRepository = new OrderDetailRepository();
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_orderDetailRepository.GetAll());
        }
    }
}
