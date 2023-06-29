using BusinessObject.Models;

namespace WebClient.ViewModels
{
    public class FoodViewModel
    {
        public List<BreadCrumb> Breadcrumbs { get; set; }
        public List<Product> Products { get; set; }
        public List<Bird> Birds { get; set; }
    }
}
