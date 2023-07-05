using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories.SearchRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchRepository _searchRepository = new SearchRepository();

        [HttpGet("{keyWord}")]
        public IActionResult Search(string keyWord)
        {
            return Ok(_searchRepository.Search(keyWord));
        }
    }
}
