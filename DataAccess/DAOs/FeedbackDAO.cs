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

        public static object GetListFeedbackByProductId(int productId)
        {
            using (var dbContext = new BirdMealOrderDBContext())
            {
                var feedbackList = dbContext.Feedbacks
                    .Join(dbContext.OrderDetails, f => f.OrderDetailId, od => od.OrderDetailId, (f, od) => new { Feedback = f, OrderDetail = od })
                    .Join(dbContext.Orders, x => x.OrderDetail.OrderId, o => o.OrderId, (x, o) => new { Feedback = x.Feedback, OrderDetail = x.OrderDetail, Order = o })
                    .Join(dbContext.Users, x => x.Order.UserId, u => u.UserId, (x, u) => new { Feedback = x.Feedback, OrderDetail = x.OrderDetail, Order = x.Order, User = u })
                    .Where(x => x.OrderDetail.ProductId == productId)
                    .Select(x => new { Feedback = x.Feedback, Rating = x.Feedback.Rating, User = x.User, Order = x.Order, OrderDetail = x.OrderDetail })
                    .ToList();
                double avgRating = 0.0;
                if (feedbackList.Count > 0)
                {
                    avgRating = (double)feedbackList.Sum(x => x.Rating) / feedbackList.Count;
                }

                int rating1Count = feedbackList.Count(x => x.Rating == 1);
                int rating2Count = feedbackList.Count(x => x.Rating == 2);
                int rating3Count = feedbackList.Count(x => x.Rating == 3);
                int rating4Count = feedbackList.Count(x => x.Rating == 4);
                int rating5Count = feedbackList.Count(x => x.Rating == 5);

                List<object> listFeedbackOfUser = new List<object>();
                foreach (var item in feedbackList)
                {
                    var order = feedbackList.FirstOrDefault(f => f.OrderDetail.OrderDetailId == item.Feedback.OrderDetailId);
                    var feedbackOfUser = new
                    {
                        User = order.User,
                        feedback = order.Feedback
                    };
                    listFeedbackOfUser.Add(feedbackOfUser);
                }
                listFeedbackOfUser.Reverse();
                var result = new
                {
                    AvgRating = avgRating,
                    Rating1Count = rating1Count,
                    Rating2Count = rating2Count,
                    Rating3Count = rating3Count,
                    Rating4Count = rating4Count,
                    Rating5Count = rating5Count,
                    FeedbackList = listFeedbackOfUser,
                    //User = feedbackList.Select(x => x.User).OrderByDescending(x => x.UserId).ToList()
                };

                return result;
            }
        }

        public static object GetListFeedbackByMeald(int mealId)
        {
            using (var dbContext = new BirdMealOrderDBContext())
            {
                var feedbackList = dbContext.Feedbacks
                    .Join(dbContext.OrderDetails, f => f.OrderDetailId, od => od.OrderDetailId, (f, od) => new { Feedback = f, OrderDetail = od })
                    .Join(dbContext.Orders, x => x.OrderDetail.OrderId, o => o.OrderId, (x, o) => new { Feedback = x.Feedback, OrderDetail = x.OrderDetail, Order = o })
                    .Join(dbContext.Users, x => x.Order.UserId, u => u.UserId, (x, u) => new { Feedback = x.Feedback, OrderDetail = x.OrderDetail, Order = x.Order, User = u })
                    .Where(x => x.OrderDetail.MealId == mealId)
                    .Select(x => new { Feedback = x.Feedback, Rating = x.Feedback.Rating, User = x.User, Order = x.Order, OrderDetail = x.OrderDetail })
                    .ToList();

                double avgRating = 0.0;
                if (feedbackList.Count > 0)
                {
                    avgRating = (double)feedbackList.Sum(x => x.Rating) / feedbackList.Count;
                }

                int rating1Count = feedbackList.Count(x => x.Rating == 1);
                int rating2Count = feedbackList.Count(x => x.Rating == 2);
                int rating3Count = feedbackList.Count(x => x.Rating == 3);
                int rating4Count = feedbackList.Count(x => x.Rating == 4);
                int rating5Count = feedbackList.Count(x => x.Rating == 5);

                List<object> listFeedbackOfUser = new List<object>();
                foreach (var item in feedbackList)
                {
                    var order = feedbackList.FirstOrDefault(f => f.OrderDetail.OrderDetailId == item.Feedback.OrderDetailId);
                    var feedbackOfUser = new
                    {
                        User = order.User,
                        feedback = order.Feedback
                    };
                    listFeedbackOfUser.Add(feedbackOfUser);
                }
                listFeedbackOfUser.Reverse();
                var result = new
                {
                    AvgRating = avgRating,
                    Rating1Count = rating1Count,
                    Rating2Count = rating2Count,
                    Rating3Count = rating3Count,
                    Rating4Count = rating4Count,
                    Rating5Count = rating5Count,
                    FeedbackList = listFeedbackOfUser,
                    //User = feedbackList.Select(x => x.User).OrderByDescending(x => x.UserId).ToList()
                };

                return result;
            }
        }
    }
}
