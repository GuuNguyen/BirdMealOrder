using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.OrderDTO
{
    public class GetOrderDTO
    {
        public int OrderId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Status { get; set; }
    }
}
