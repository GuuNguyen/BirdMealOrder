using BusinessObject.Models;

namespace WebClient.ViewModels
{
    public class ListFeedbackViewModel
    {
        public double AvgRating { get; set; }
        public int Rating1Count { get; set; }
        public int Rating2Count { get; set; }
        public int Rating3Count { get; set; }
        public int Rating4Count { get; set; }
        public int Rating5Count { get; set; }
        public List<FeedbackOfUser> FeedbackList { get; set; }
        //public List<User> User { get; set; }
    }
    public class FeedbackOfUser
    {
        public Feedback Feedback { get; set; }
        public User user { get; set; }
    }
}
