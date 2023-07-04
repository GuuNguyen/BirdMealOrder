using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class FeedbackDAO
    {
        public static void Create(Feedback feedback)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    context.Feedbacks.Add(feedback);
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
