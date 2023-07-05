using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.SortDTO;
using Repositories.Repositories.SortRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SortController : ControllerBase
    {
        private readonly ISortRepository _repo;

        public SortController(ISortRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult ApplySort(SortInputDTO value)
        {
            return Ok(_repo.SortList(value));   
        }
    }
}
