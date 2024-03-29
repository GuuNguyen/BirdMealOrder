﻿using BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            ShippingAddresses = new HashSet<ShippingAddress>();
        }

        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int RoleId { get; set; }
        public UserStatus Status { get; set; }
        public virtual Role Role { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
        [JsonIgnore]
        public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; }
    }
}
