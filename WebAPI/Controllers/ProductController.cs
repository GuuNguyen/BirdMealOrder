using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.ProductDTO;
using Repositories.Repositories.ProductRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var listProduct = _repo.GetAllProducts();
            return Ok(listProduct);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductByID(int id)
        {
            var product = _repo.GetProductById(id);
            return Ok(product);
        }

        [HttpGet("ByMeal/{mealId}")]
        public IActionResult GetProductsByMeal(int mealId)
        {
            var product = _repo.GetProductsByMealId(mealId);
            return Ok(product);
        }

        [HttpGet("Detail/{productCode}")]
        public IActionResult GetProductByID(string productCode)
        {
            return Ok(_repo.GetProductByCode(productCode));
        }

        [HttpPost("Products")]
        public IActionResult GetProductByIds(List<int> productIds)
        {
            var isSuccess = _repo.GetProductsByIds(productIds);
            return isSuccess != null ? Ok(isSuccess) : BadRequest("Fail create!");
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateProduct(CreateProductDTO product)
        {
            var isSuccess = _repo.AddProduct(product);
            return isSuccess ? Ok("Successfull created!") : BadRequest("Fail create!");
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateProduct(ProductDTO product)
        {
            var isSuccess = _repo.UpdateProduct(product);
            return isSuccess ? Ok("Successfull update!") : BadRequest("Fail update!");
        }
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteProduct(int id)
        {
            var isSuccess = _repo.DeleteProduct(id);
            return isSuccess ? Ok("Successfull delete!") : BadRequest("Fail delete!");
        }

        [HttpPut("changeStatus/{id}")]
        [Authorize]
        public IActionResult ChangeStatus(int id)
        {
            var isSuccess = _repo.ChangeProductStatus(id);
            return isSuccess ? Ok("Successfull change!") : BadRequest("Fail change!");
        }
    }
}
