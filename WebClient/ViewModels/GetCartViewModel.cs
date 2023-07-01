using BusinessObject;
using BusinessObject.Models;

namespace WebClient.ViewModels
{
    public class GetCartViewModel
    {
        public List<Meal>? Meals { get; set; }
        public List<Product>? Products { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}
