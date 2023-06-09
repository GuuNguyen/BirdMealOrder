using BusinessObject.Models;
using Repositories.DTOs.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.OrderRepositories
{
    public interface IOrderRepository
    {
        List<Order> GetOrders();
        bool CreateOrder(CreateOrderDTO order);
        bool UpdateOrder(UpdateOrderDTO order);
        bool DeleteOrder(int id);
    }
}
