﻿using BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.OrderDTO
{
    public class ChangeOrderStatusDTO
    {
        public int OrderId { get; set; }
        public DateTime? ShipDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public OrderStatus Status { get; set; }
    }
}
