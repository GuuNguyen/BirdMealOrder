using BusinessObject.Models;

namespace WebClient.ViewModels
{
    public class MealViewModel
    {
        public List<BreadCrumb> Breadcrumbs { get; set; }
        public List<Meal> Meals { get; set; }
    }
}
