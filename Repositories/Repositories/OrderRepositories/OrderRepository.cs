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

        public bool ChangeOrderStatus(ChangeOrderStatusDTO order)
        {
            var check = OrderDAO.GetOrder(order.OrderId);
            if (check == null) return false;
            if (order.Status == OrderStatus.Processing)
            {
                order.ShipDate = DateTime.Now;
            }
            else if(order.Status == OrderStatus.Completed)
            {
                order.ShipDate = check.ShipDate;
                order.CompleteDate = DateTime.Now;
            }
            else if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Canceled)
            {
                order.ShipDate = check.ShipDate;
                if (order.Status == OrderStatus.Canceled)
                {
                    var orderDetails = OrderDetailDAO.ListOderDetailByOderId(order.OrderId);
                    Dictionary<int, int> productQuantityRefund = new Dictionary<int, int>();
                    Dictionary<int, int> mealQuantityRefund = new Dictionary<int, int>();
                    foreach (var o in orderDetails)
                    {
                        if (o.ProductId != null)
                        {
                            productQuantityRefund.Add((int)o.ProductId, o.Quantity);
                            var checkRefund = ProductDAO.RefundQuantityProduct(productQuantityRefund);
                            if (!checkRefund) return false;
                        }
                        if (o.MealId != null)
                        {
                            mealQuantityRefund.Add((int)o.MealId, o.Quantity);
                            var checkRefund = MealDAO.RefundQuantityMeal(mealQuantityRefund);
                            if (!checkRefund) return false;
                        }
                    }
                }
            }
            else
            {
                order.ShipDate = new DateTime(1753, 1, 1);
                order.CompleteDate = null;
            }
            var updateOrder = _mapper.Map(order, check);
            OrderDAO.ChangeOrderStatus(updateOrder);
            return true;
        }

        public bool DeleteChangeOrderStatus(int id)
        {
            var check = OrderDAO.GetOrder(id);
            if (check == null) return false;
            OrderDAO.DeleteChangeOrderStatus(id);
            return true;
        }

        public List<object> GetListBestSeller()
        {
            var listOrder = OrderDetailDAO.GetListAllOrderDetail();
            var productDictionary = GetProductDictionary();
            var mealDictionary = GetMealDictionary();
            Dictionary<string, int> productQuantities = new Dictionary<string, int>();

            foreach (var orderDetail in listOrder)
            {
                int quantity = orderDetail.Quantity;
                string product = "";

                if (orderDetail.ProductId.HasValue && productDictionary.ContainsKey(orderDetail.ProductId.Value))
                {
                    product = productDictionary[orderDetail.ProductId.Value].ProductCode;
                }
                else if (orderDetail.MealId.HasValue && mealDictionary.ContainsKey(orderDetail.MealId.Value))
                {
                    product = mealDictionary[orderDetail.MealId.Value].MealCode;
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
                switch (prefix)
                {
                    case "ME":
                        var meal = mealDictionary.Values.FirstOrDefault(m => m.MealCode == pair.Key);
                        bestSellers.Add(meal);
                        break;
                    case "FO":
                        var product = productDictionary.Values.FirstOrDefault(p => p.ProductCode == pair.Key);
                        bestSellers.Add(product);
                        break;
                }
            }
            return bestSellers;
        }


        public Dictionary<int, Meal> GetMealDictionary()
        {
            var mealList = MealDAO.GetAllMeals();
            var mealDictionary = new Dictionary<int, Meal>();

            foreach (var meal in mealList)
            {
                mealDictionary.Add(meal.MealId, meal);
            }

            return mealDictionary;
        }

        public Dictionary<int, Product> GetProductDictionary()
        {
            var productList = ProductDAO.GetAllProducts();
            var productDictionary = new Dictionary<int, Product>();

            foreach (var product in productList)
            {
                productDictionary.Add(product.ProductId, product);
            }

            return productDictionary;
        }


        public List<object> GetListRecommend()
        {
            var productDictionary = GetProductDictionary();
            var mealDictionary = GetMealDictionary();
            var productCounts = (from f in _context.Feedbacks
                                 join od in _context.OrderDetails on f.OrderDetailId equals od.OrderDetailId
                                 where f.Rating == 5 && od.ProductId != null
                                 group f by od.ProductId into g
                                 select new
                                 {
                                     Id = g.Key,
                                     Count = g.Count()
                                 }).ToList();

            var mealCounts = (from f in _context.Feedbacks
                              join od in _context.OrderDetails on f.OrderDetailId equals od.OrderDetailId
                              where f.Rating == 5 && od.MealId != null
                              group f by od.MealId into g
                              select new
                              {
                                  Id = g.Key,
                                  Count = g.Count()
                              }).ToList();

            var topProducts = productCounts
                .OrderByDescending(pc => pc.Count)
                .Select(pc => productDictionary[pc.Id.Value])
                .ToList();

            var topMeals = mealCounts
                .OrderByDescending(mc => mc.Count)
                .Select(mc => mealDictionary[mc.Id.Value])
                .ToList();

            var listRecommend = new List<object>();

            listRecommend.AddRange(topProducts);
            listRecommend.AddRange(topMeals);

            return listRecommend.Take(8).ToList();
        }

    }
}
