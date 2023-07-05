using BusinessObject.Models;

namespace WebClient.ViewModels
{
    public class DetailPMViewModel
    {
        public List<BreadCrumb> Breadcrumbs { get; set; }
        public List<Product> Products { get; set; }
        public List<Meal> Meals { get; set; }
        public List<Product> ProductIngredients { get; set; }
        public List<Bird> Birds { get; set; }
        public Meal? Meal { get; set; }
        public Product? Product { get; set; }
        public List<MealProduct> MealProducts { get; set; }

        public ListFeedbackViewModel ListFeedbackViewModel { get; set; }
    }
}
