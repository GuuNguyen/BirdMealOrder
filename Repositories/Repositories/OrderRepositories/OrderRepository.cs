using AutoMapper;
using BusinessObject.Enums;
using BusinessObject.Models;
using DataAccess.DAOs;
using Repositories.DTOs.OrderDTO;


namespace Repositories.Repositories.OrderRepositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMapper _mapper;
        private readonly BirdMealOrderDBContext _context;

        public OrderRepository(IMapper mapper, BirdMealOrderDBContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public List<Order> GetOrders() => OrderDAO.GetOrders();

        public bool CreateOrder(CreateOrderDTO order)
        {
            if (order == null) return false;
            decimal total = 0;
            var newOrder = new Order
            {
                UserId = order.UserId,
                ShippingAddressId = order.ShippingAddressId,
                OrderDate = DateTime.Now,
                ShipDate = null,
                Status = OrderStatus.Pending,
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
                    _context.Products.Update(product);
                }
                else if (cartItem.MealId != null)
                {
                    var meal = MealDAO.GetMeal((int)cartItem.MealId);
                    orderDetail.UnitPrice = meal.Price;
                    meal.QuantityAvailable -= cartItem.Quantity;
                    _context.Meals.Update(meal);
                }
                newOrder.OrderDetails.Add(orderDetail);
                _context.OrderDetails.Add(orderDetail);
            }
            foreach (var i in newOrder.OrderDetails)
            {
                total += (i.UnitPrice * i.Quantity);
            }
            newOrder.TotalPrice = total;
            _context.Orders.Add(newOrder);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateOrder(UpdateOrderDTO order)
        {
            if (order == null) return false;
            var updateOrder = _mapper.Map<Order>(order);
            OrderDAO.Update(updateOrder);
            return true;
        }

        public bool DeleteOrder(int id)
        {
            var order = OrderDAO.GetOrder(id);
            if (order == null) return false;
            OrderDAO.DeleteOrder(id);
            return true;
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return OrderDAO.GetOrdersByUserId(userId);
        }

        public Order GetOrder(int id)
        {
            return OrderDAO.GetOrder(id);
        }

        public List<object> GetOrdersAndCheckHasReviewByUserId(int userId)
        {
            return OrderDAO.GetOrdersAndCheckHasReviewByUserId(userId);
        }

        public List<object> GetListBestSeller()
        {
            var listOrder = OrderDetailDAO.GetListAllOrderDetail();
            Dictionary<string, int> productQuantities = new Dictionary<string, int>();

            foreach (var orderDetail in listOrder)
            {
                int quantity = orderDetail.Quantity;
                string product = "";

                if (orderDetail.ProductId.HasValue)
                {
                    product = ProductDAO.GetProductById(orderDetail.ProductId.Value).ProductCode;
                }
                else
                {
                    product = MealDAO.GetMeal(orderDetail.MealId.Value).MealCode;
                }

                if (!string.IsNullOrEmpty(product))
                {
                    if (productQuantities.ContainsKey(product))
                    {
                        productQuantities[product] += quantity;
                    }
                    else
                    {
                        productQuantities.Add(product, quantity);
                    }
                }
            }

            var sortedProducts = productQuantities.OrderByDescending(pair => pair.Value);

            List<object> bestSellers = new List<object>();
            foreach (var pair in sortedProducts)
            {
                string prefix = pair.Key.Substring(0, 2);
                switch(prefix)
                {
                    case "ME":
                        var meal = MealDAO.GetMealByCode(pair.Key); 
                        bestSellers.Add(meal);
                        break;
                    case "FO":
                        var product = ProductDAO.GetProductByCode(pair.Key);
                        bestSellers.Add(product);
                        break;
                }
            }
            return bestSellers;
        }

        public List<object> GetListRecommend()
        {
            var productList = (from od in _context.OrderDetails
                               join f in _context.Feedbacks on od.OrderDetailId equals f.OrderDetailId
                               where f.Rating == 5 && od.ProductId != null
                               group od by od.ProductId into g
                               orderby g.Count() descending
                               select new
                               {
                                   ProductId = g.Key,
                                   Count = g.Count()
                               }).ToList();

            var mealList = (from od in _context.OrderDetails
                            join f in _context.Feedbacks on od.OrderDetailId equals f.OrderDetailId
                            where f.Rating == 5 && od.MealId != null
                            group od by od.MealId into g
                            orderby g.Count() descending
                            select new
                            {
                                MealId = g.Key,
                                Count = g.Count()
                            }).ToList();

            var listRecommend = new List<object>();
            foreach (var item in productList)
            {
                var product = ProductDAO.GetProductById(item.ProductId.Value);
                if (product != null)
                {
                    listRecommend.Add(product);
                }
            }

            foreach (var item in mealList)
            {
                var meal = MealDAO.GetMeal(item.MealId.Value);
                if (meal != null)
                {
                    listRecommend.Add(meal);
                }
            }

            return listRecommend;
        }

    }
}
