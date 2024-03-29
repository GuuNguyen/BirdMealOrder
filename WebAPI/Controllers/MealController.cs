﻿
using DataAccess.DAOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repositories.DTOs.MealDTO;
using Repositories.Repositories.MealRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealRepository _mealRepo;

        public MealController(IMealRepository mealRepository)
        {
            _mealRepo = mealRepository;
        }
        [HttpGet]
        public IActionResult GetAllMeals()
        {
            return Ok(_mealRepo.GetAllMeals());
        }

        [HttpGet("{id}")]
        public IActionResult GetMeal(int id)
        {
            return Ok(_mealRepo.GetMeal(id));
        }

        [HttpGet("Detail/{mealCode}")]
        public IActionResult GetMeal(string mealCode)
        {
            return Ok(_mealRepo.GetMealByCode(mealCode));
        }

        [HttpGet("ByBirdId/{birdId}")]
        public IActionResult GetMealsByBirdId(int birdId)
        {
            return Ok(_mealRepo.GetMealsByBirdId(birdId));
        }

        [HttpPost("Meals")]
        public IActionResult CreateMeal(List<int> mealIds)
        {
            var check = _mealRepo.GetMealsByIds(mealIds);
            if (check.IsNullOrEmpty()) return BadRequest("Fail");
            return Ok(check);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteMeal(int id)
        {
            var meal = _mealRepo.GetMeal(id);
            if (meal == null) return NotFound();
            _mealRepo.DeleteMeal(id);
            return Ok("Delete Successfull!");
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateMeal(CreateMealDTO mealDTO)
        {
            var check = _mealRepo.CreateMeal(mealDTO);
            if (!check.IsNullOrEmpty()) return Conflict(check);
            return Ok("Create Successfull!");
        }
        [Authorize]
        [HttpPut]
        public IActionResult UpdateMeal(UpdateMealDTO mealDTO)
        {
            var meal = _mealRepo.GetMeal(mealDTO.MealId);
            if (meal == null) return NotFound();
            var check = _mealRepo.UpdateMeal(mealDTO);
            if (!check.IsNullOrEmpty()) return Conflict(check);
            return Ok("Update Successfull!");
        }
        [Authorize]
        [HttpPut("ChangeStatus/{mealId}")]
        public IActionResult ChangeStatus(int mealId)
        {
            var meal = _mealRepo.GetMeal(mealId);
            if (meal == null) return NotFound();
            _mealRepo.ChangeStatus(mealId);
            return Ok("Change Successfull!");
        }

        [HttpGet("GetMealIncludeBird&Product/{mealId}")]
        public IActionResult GetMealIncludeBirdAndProduct(int mealId)
        {
            var meal = _mealRepo.GetMeal(mealId);
            if (meal == null) return NotFound();
            return Ok(_mealRepo.GetMealInclueBirdAndProduct(mealId));
        }


        [HttpGet("GetMealProductByMealId/{mealId}")]
        public IActionResult GetMealProductByMealId(int mealId)
        {
            var meal = _mealRepo.GetMeal(mealId);
            if (meal == null) return NotFound();
            return Ok(_mealRepo.GetMealProductsByMealId(mealId));
        }
    }
}
