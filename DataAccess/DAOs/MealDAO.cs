﻿using BusinessObject.Models;
using System;
using System.Collections.Generic;
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
                    context.Remove(m);
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
                    context.Meals.Update(mealUpdate);
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
