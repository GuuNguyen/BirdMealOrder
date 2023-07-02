using WebClient.Models;

namespace WebClient.ViewModels
{
    public class ShippingAddressViewModel
    {
        public List<City> Cities { get; set; }  
        public string? SelectedCity { get; set; }
        public string? SelectedDistrict { get; set; }
        public string? SelectedWard { get; set; }
    }
}
