using BusinessObject.Models;

namespace WebClient.ViewModels
{
    public class MealViewModel
    {
        public List<BreadCrumb> Breadcrumbs { get; set; }
        public List<Meal> Meals { get; set; }
        public List<Bird> Birds { get; set; }
        public string PageType { get; set; }
    }
}
