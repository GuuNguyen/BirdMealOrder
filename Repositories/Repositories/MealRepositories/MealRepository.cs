using AutoMapper;
using BusinessObject.Models;
using DataAccess.DAOs;
using Repositories.DTOs.MealDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.MealRepositories
{
    public class MealRepository : IMealRepository
    {
        private readonly IMapper _mapper;

        public MealRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public void CreateMeal(CreateMealDTO createMeal)
        {
            var meal = _mapper.Map<Meal>(createMeal);
            MealDAO.Create(meal);
        }

        public void DeleteMeal(int id)
        {
            MealDAO.Delete(id);
        }

        public List<Meal> GetAllMeals()
        {
            return MealDAO.GetAllMeals();
        }

        public Meal GetMeal(int id)
        {
            return MealDAO.GetMeal(id);
        }

        public void UpdateMeal(MealDTO mealDTO)
        {
            var meal = _mapper.Map<Meal>(mealDTO);
            MealDAO.Update(meal);
        }
    }
}
