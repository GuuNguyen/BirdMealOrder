﻿using AutoMapper;
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
            if(order.Status == OrderStatus.Processing)
            {
                order.ShipDate = DateTime.Now.AddDays(1);
            }
            else if(order.Status == OrderStatus.Completed)
            {
                order.ShipDate = check.ShipDate;
            }
            else
            {
                order.ShipDate = new DateTime(1753, 1, 1);
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
    }
}
