using AutoMapper;
using BusinessObject.Enums;
using BusinessObject.Models;
using Repositories.DTOs.SortDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.SortRepositories
{
    public class SortRepository : ISortRepository
    {
        private readonly BirdMealOrderDBContext _context;

        public SortRepository(BirdMealOrderDBContext context)
        {
            _context = context;
        }

        public List<object> SortList(SortInputDTO values)
        {
            var sortedList = new List<object>();
            if (values.PageType == "Meal")
            {              
                if(values?.SelectedPrice?.Only == 0 && values.SelectedBirds != null)
                {
                    sortedList = (from bm in _context.BirdMeals
                                 join m in _context.Meals on bm.MealId equals m.MealId
                                 where m.MealStatus == MealStatus.Available 
                                 && m.Price >= values.SelectedPrice.Min 
                                 && m.Price < values.SelectedPrice.Max
                                 && values.SelectedBirds.Contains(bm.BirdId)
                                  select new Meal
                                 {
                                     MealId = m.MealId,
                                     MealCode = m.MealCode,
                                     MealDescription = m.MealDescription,
                                     MealImage = m.MealImage,
                                     MealName = m.MealName,
                                     MealStatus = m.MealStatus,
                                     Price = m.Price,
                                     QuantityAvailable = m.QuantityAvailable,
                                 }).Cast<object>().Distinct().ToList();
                } 
                else if(values?.SelectedPrice?.Only != 0 && values.SelectedBirds != null)
                {
                    sortedList = (from bm in _context.BirdMeals
                                  join m in _context.Meals on bm.MealId equals m.MealId
                                  where m.MealStatus == MealStatus.Available
                                  && m.Price >= values.SelectedPrice.Only
                                  && values.SelectedBirds.Contains(bm.BirdId)
                                  select new Meal
                                  {
                                      MealId = m.MealId,
                                      MealCode = m.MealName,
                                      MealDescription = m.MealDescription,
                                      MealImage = m.MealImage,
                                      MealName = m.MealName,
                                      MealStatus = m.MealStatus,
                                      Price = m.Price,
                                      QuantityAvailable = m.QuantityAvailable,
                                  }).Cast<object>().Distinct().ToList();
                }else if(values?.SelectedPrice?.Only == 0 && values.SelectedBirds == null)
                {
                    sortedList = _context.Meals.Where(m => m.Price >= values.SelectedPrice.Min
                                                     && m.Price < values.SelectedPrice.Max)
                                                    .Cast<object>().Distinct().ToList();
                }
                else
                {
                    sortedList = _context.Meals.Where(m => m.Price >= values.SelectedPrice.Only)
                                                    .Cast<object>().Distinct().ToList();
                }
            }
            else if(values.PageType == "Food")
            {
                if(values.SelectedPrice?.Only == 0)
                {
                    sortedList = _context.Products.Where(p => p.Price >= values.SelectedPrice.Min 
                                                        && p.Price < values.SelectedPrice.Max)
                                                        .Cast<object>().Distinct().ToList();
                }
                else
                {
                    sortedList = _context.Products.Where(m => m.Price >= values.SelectedPrice.Only)
                                                    .Cast<object>().Distinct().ToList();
                }
            }
            return sortedList;
        }
    }
}
