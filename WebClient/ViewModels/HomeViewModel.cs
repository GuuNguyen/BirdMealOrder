using BusinessObject.Models;

namespace WebClient.VIewModels
{
    public class HomeViewModel
    {
        public List<Meal> Meals { get; set; }
        public List<Bird> Birds { get; set; }

        public List<object> BestSellers { get; set; }
        public List<object> HighlyRecommends { get; set; }
    }
}
