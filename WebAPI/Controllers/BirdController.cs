using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories.BirdRepositories;
using Repositories.Repositories.MealRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirdController : ControllerBase
    {
        private readonly IBirdRepository _birdRepo;

        public BirdController(IBirdRepository birdRepository)
        {
            _birdRepo = birdRepository;
        }
        [HttpGet]
        public IActionResult GetAllBirds()
        {
            return Ok(_birdRepo.GetAllBirds());
        }
    }
}
