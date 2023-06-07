using BusinessObject.Models;
using DataAccess.DAOs;
using Repositories.DTOs.OrderDTO;


namespace Repositories.Repositories.OrderRepositories
{
    public class OrderRepository : IOrderRepository
    {
        public List<Order> GetOrders() => OrderDAO.GetOrders();

        public bool CreateOrder(CreateOrderDTO order)
        {           
            if (order == null) return false;
            decimal total = 0;
            var newOrder = new Order
            {
                UserId = order.UserId,
                OrderDate = DateTime.Now,
                ShipDate = null,
                Status = 0,
                OrderDetails = new List<OrderDetail>()
            };
            foreach (var cartItem in order.CartItems)
            {
                var orderDetail = new OrderDetail
                {
                    MealId = cartItem.MealId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = 0
                };
                if (cartItem.ProductId != null)
                {
                    var product = ProductDAO.GetProductById((int)cartItem.ProductId);
                    orderDetail.UnitPrice = product.Price;
                    product.QuantityAvailable -= cartItem.Quantity;
                    ProductDAO.UpdateProduct(product);
                }
                else if (cartItem.MealId != null)
                {
                    var meal = MealDAO.GetMeal((int)cartItem.MealId);
                    orderDetail.UnitPrice = meal.Price;
                    meal.QuantityAvailable -= cartItem.Quantity;
                    MealDAO.Update(meal);                    
                }
                newOrder.OrderDetails.Add(orderDetail);
                OrderDetailDAO.CreateOrderDetail(orderDetail);                
            }
            foreach (var i in newOrder.OrderDetails)
            {
                total += (i.UnitPrice * i.Quantity);
            }
            newOrder.TotalPrice = total;
            OrderDAO.CreateOrder(newOrder);
            return true;
        }
    }
}
