using BusinessObject;
using BusinessObject.Models;
using Repositories.DTOs.ShippingAddressDTO;

namespace WebClient.ViewModels
{
    public class GetCartViewModel
    {
        public List<Meal>? Meals { get; set; }
        public List<Product>? Products { get; set; }
        public List<CartItem>? CartItems { get; set; }
        public List<GetSADTO>? ShippingAddresses { get; set; }
    }
}
