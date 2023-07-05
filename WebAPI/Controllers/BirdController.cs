using BusinessObject.Models;
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
        
        [HttpGet("{id}")]
        public IActionResult GetBirdByID(int id)
        {
            var bird = _birdRepo.GetBirdById(id);
            if (bird == null)
            {
                return NotFound("No bird have this birdId");
            }
            return Ok(bird);
        }

        [HttpGet("ByMeal/{mealId}")]
        public IActionResult GetBirdsByMealId(int mealId)
        {
            var bird = _birdRepo.GetBirdsByMealId(mealId);
            if (bird == null)
            {
                return NotFound("No bird have this birdId");
            }
            return Ok(bird);
        }

        [HttpPost]
        public IActionResult CreateBird(Bird bird)
        {
            var b = _birdRepo.CreateBird(bird);
            return Ok("Create Successfull!");
        }

        [HttpPut]
        public IActionResult UpdateBird(Bird bird)
        {
            var b = _birdRepo.GetBirdById(bird.BirdId);
            if(b == null)
            {
                return NotFound("No bird have this birdId");
            }
            _birdRepo.UpdateBird(bird);
            return Ok("Update Successfull!");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var b = _birdRepo.GetBirdById(id);
            if(b == null)
            {
                return NotFound("No bird have this birdId");
            }
            _birdRepo.DeleteBird(id);
            return Ok("Delete Successfull!");
        }
    }
}
