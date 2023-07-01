﻿using BusinessObject.Enums;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class MealDAO
    {
        public static List<Meal> GetAllMeals()
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.Meals.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Meal GetMeal(int id)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.Meals.Find(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Meal GetMealByCode(string code)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.Meals.Where(m => m.MealCode == code).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<Meal> GetMealsByIds(List<int> mealIds)
        {
            try
            {
                using(var context = new BirdMealOrderDBContext())
                {
                    return context.Meals.Where(m => mealIds.Contains(m.MealId)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int Create(Meal meal)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    context.Meals.Add(meal);
                    context.SaveChanges();

                    return meal.MealId; // Trả về ID của bữa ăn vừa tạo
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static void Delete(int id)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var m = context.Meals.Find(id);
                    context.Meals.Remove(m);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Update(Meal mealUpdate)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var meal = context.Meals.FirstOrDefault(m => m.MealId == mealUpdate.MealId);
                    meal.MealCode = mealUpdate.MealCode;
                    meal.MealName = mealUpdate.MealName;
                    meal.MealDescription = mealUpdate.MealDescription;
                    meal.Price = mealUpdate.Price;
                    meal.MealStatus = mealUpdate.MealStatus;
                    meal.QuantityAvailable = mealUpdate.QuantityAvailable;
                    meal.MealImage = mealUpdate.MealImage;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void ChangeStatus(int mealId)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var meal = context.Meals.Find(mealId);
                    if (meal.MealStatus == 0)
                    {
                        meal.MealStatus = 1;
                    }
                    else
                    {
                        meal.MealStatus = 0;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
