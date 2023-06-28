using AutoMapper;
using BusinessObject.Models;
using DataAccess.DAOs;
using Microsoft.IdentityModel.Tokens;
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
        public string CreateMeal(CreateMealDTO createMeal)
        {
            foreach (ProductRequired productRequired in createMeal.ProductOptions)
            {
                string mess = MealProductDAO.CheckQuantityProductAvailable(productRequired.ProductId, productRequired.QuantityRequired);
                if (!mess.IsNullOrEmpty())
                {
                    return mess;
                }
            }
            var meal = _mapper.Map<Meal>(createMeal);
            meal.MealStatus = MealStatus.Available;
            int mealId = MealDAO.Create(meal);
            if (mealId > 0)
            {
                var birdMeals = createMeal.BirdIds.Select(id => new BirdMeal
                {
                    BirdId = id,
                    MealId = mealId
                }).ToList();
                BirdMealDAO.CreateRange(birdMeals);

                var mealProducts = createMeal.ProductOptions.Select(productRequired => new MealProduct
                {
                    ProductId = productRequired.ProductId,
                    QuantityRequired = productRequired.QuantityRequired,
                    MealId = mealId
                }).ToList();
                MealProductDAO.CreateRange(mealProducts);
            }
            return string.Empty;
        }

        public void DeleteMeal(int id)
        {
            var mealInOrderDetails = OrderDetailDAO.CheckMealInOderDetails(id);
            if (mealInOrderDetails)
            {
                var meal = MealDAO.GetMeal(id);
                meal.MealStatus = MealStatus.Unavailable;
                MealDAO.Update(meal);
            }
            else
            {
                MealDAO.Delete(id);
            }

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
