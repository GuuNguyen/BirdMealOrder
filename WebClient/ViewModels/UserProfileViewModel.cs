using BusinessObject.Models;
using Repositories.DTOs.ShippingAddressDTO;

namespace WebClient.ViewModels
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public List<GetSADTO>? ShippingAddresses { get; set; }
    }
}
