using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.OrderDTO
{
    public class CreateOrderDTO
    {
        public int UserId { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
