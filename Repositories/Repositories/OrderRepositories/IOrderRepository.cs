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
        List<Order> GetOrdersByUserId(int userId);
        Order GetOrder(int id);
        List<object> GetOrdersAndCheckHasReviewByUserId(int userId);
        List<object> GetListBestSeller();
        List<object> GetListRecommend();
    }
}
