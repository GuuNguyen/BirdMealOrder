using AutoMapper;
using BusinessObject.Enums;
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

        public void ChangeStatus(int mealId)
        {
            MealDAO.ChangeStatus(mealId);
        }

        public string CreateMeal(CreateMealDTO createMeal)
        {
            foreach (ProductRequired productRequired in createMeal.ProductOptions)
            {
                string mess = MealProductDAO.CheckQuantityProductAvailable(productRequired.ProductId, productRequired.QuantityRequired * createMeal.QuantityAvailable);
                if (!mess.IsNullOrEmpty())
                {
                    return mess;
                }
            }
            var meal = _mapper.Map<Meal>(createMeal);
            meal.MealCode = GenerateCodeDAO.GenerateProductCode("Meal");
            meal.MealStatus = (int)MealStatus.Available;
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
                MealProductDAO.CreateRange(mealProducts, createMeal.QuantityAvailable);
            }
            return string.Empty;
        }

        public void DeleteMeal(int id)
        {
            var mealInOrderDetails = OrderDetailDAO.CheckMealInOderDetails(id);
            var meal = MealDAO.GetMeal(id);
            if (mealInOrderDetails)
            {
                meal.MealStatus = (int)MealStatus.Unavailable;
                MealDAO.Update(meal);
            }
            else
            {
                Dictionary<int, int> productQuantityRefund = new Dictionary<int, int>();
                var mealProducts = MealProductDAO.GetProductsByMealId(id);
                foreach (var productMeal in mealProducts)
                {
                    productQuantityRefund.Add(productMeal.ProductId, productMeal.QuantityRequired * meal.QuantityAvailable);
                }
                var checkRefund = ProductDAO.RefundQuantityProduct(productQuantityRefund);
                if (checkRefund) MealDAO.Delete(id);
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
