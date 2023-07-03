using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class ShippingAddress
    {
        public ShippingAddress()
        {
            Orders = new HashSet<Order>();
        }

        public int ShippingAddressId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public string StreetAddress { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
