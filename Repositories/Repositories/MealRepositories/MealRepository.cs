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
        private readonly BirdMealOrderDBContext _dbContext;
        public MealRepository(IMapper mapper, BirdMealOrderDBContext dBContext)
        {
            _mapper = mapper;
            _dbContext = dBContext;
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

        public UpdateMealDTO? GetMealInclueBirdAndProduct(int mealId)
        {
            try
            {
                var productRequireds = _dbContext.MealProducts.Where(mp => mp.MealId == mealId)
                                        .Select(mp => new ProductRequired
                                        {
                                            ProductId = mp.ProductId,
                                            QuantityRequired = mp.QuantityRequired
                                        }).ToList();

                var birdIds = _dbContext.BirdMeals.Where(b => b.MealId == mealId)
                                        .Select(b => b.BirdId).ToList();

                return (from m in _dbContext.Meals
                        where m.MealId == mealId
                        select new UpdateMealDTO
                        {
                            MealId = m.MealId,
                            MealCode = m.MealCode,
                            MealName = m.MealName,
                            MealDescription = m.MealDescription,
                            Price = m.Price,
                            QuantityAvailable = m.QuantityAvailable,
                            MealImage = m.MealImage,
                            MealStatus = (MealStatus)m.MealStatus,
                            ProductRequireds = productRequireds,
                            BirdIds = birdIds
                        }).SingleOrDefault();


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Meal GetMealByCode(string code) => MealDAO.GetMealByCode(code);
        public string UpdateMeal(UpdateMealDTO mealDTO)
        {
            var oldMeal = MealDAO.GetMeal(mealDTO.MealId);
            var meal = _mapper.Map<Meal>(mealDTO);
            var productsInMealOld = MealProductDAO.GetProductsByMealId(mealDTO.MealId);
            foreach (var productUpdate in mealDTO.ProductRequireds)
            {
                var product = productsInMealOld.FirstOrDefault(p => p.ProductId == productUpdate.ProductId);
                if (product != null)
                {
                    // lay so luong moi tru so luong cu
                    var newQuantityUpdate = productUpdate.QuantityRequired * mealDTO.QuantityAvailable - product.QuantityRequired * oldMeal.QuantityAvailable;
                    if (newQuantityUpdate > 0) // neu tang kiem tra product trong kho
                    {
                        var check = MealProductDAO.CheckQuantityProductAvailable(productUpdate.ProductId, newQuantityUpdate);
                        if (!check.IsNullOrEmpty()) return check;

                    }
                    var mealProduct = new MealProduct
                    {
                        MealId = mealDTO.MealId,
                        ProductId = productUpdate.ProductId,
                        QuantityRequired = productUpdate.QuantityRequired,
                    };
                    MealProductDAO.Update(mealProduct, newQuantityUpdate);
                }
            }
            var birdMeals = mealDTO.BirdIds.Select(id => new BirdMeal
            {
                BirdId = id,
                MealId = mealDTO.MealId
            }).ToList();

            BirdMealDAO.DeleteByMealId(mealDTO.MealId);
            BirdMealDAO.CreateRange(birdMeals);
            MealDAO.Update(meal);

            return string.Empty;
        }

        public List<Meal> GetMealsByIds(List<int> mealIds) => MealDAO.GetMealsByIds(mealIds);
    }
}
