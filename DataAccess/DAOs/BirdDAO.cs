using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class BirdDAO
    {
        public static List<Bird> GetAllBirds()
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.Birds.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Bird GetBirdById(int id)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.Birds.Find(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateBird(Bird bird)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    context.Birds.Add(bird);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateBird(Bird bird)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var birdUpdaet =  context.Birds.FirstOrDefault(p => p.BirdId == bird.BirdId);
                    birdUpdaet.BirdName = bird.BirdName;
                    birdUpdaet.BirdImage = bird.BirdImage;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteBird(int id)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var birdDelete = context.Birds.Find(id);
                    context.Remove(birdDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
