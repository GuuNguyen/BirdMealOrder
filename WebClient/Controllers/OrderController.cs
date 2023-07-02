using BusinessObject.Enums;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebClient.ViewModels;

namespace WebClient.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient client;
        private string OrderApiUrl = "";

        public OrderController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "https://localhost:7022/api/Order";
        }
        public async Task<IActionResult> Order_Index()
        {
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Order> listOrder = JsonSerializer.Deserialize<List<Order>>(strData, options);
            return View(listOrder);
        }
        [HttpGet]
        public async Task<IActionResult> OrderHistory()
        {
            var userId = HttpContext.Session.GetInt32("userID");
            if (userId == null)
            {
                return Redirect("/Login/Login");
            }
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl + "GetOrderByUserId/" + userId);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<OrderHistoryViewModel> listOrder = new List<OrderHistoryViewModel>();
            if (!string.IsNullOrEmpty(strData))
            {
                listOrder = JsonSerializer.Deserialize<List<OrderHistoryViewModel>>(strData, options);
            }
            else
            {
                var product = new Product
                {
                    ProductId = 1,
                    Price = 15,
                    Description = "alo alo",
                    ProductCode = "P123",
                    ProductImage = "https://binhminhdigital.com/StoreData/PageData/3429/Tim-hieu-ve-ban-quyen-hinh-anh%20(3).jpg",
                    ProductName = "Khi111111111111111",
                    ProductStatus = ProductStatus.Available,
                    QuantityAvailable = 15,
                };
                var meal = new Meal
                {
                    MealId = 1,
                    MealName = "Meal",
                    MealDescription = "Meal Meal",
                    MealCode = "M123",
                    Price = 25,
                    QuantityAvailable = 20,
                    MealStatus = MealStatus.Available,
                    MealImage = "https://vapa.vn/wp-content/uploads/2022/12/anh-cute-001-1.jpg",
                };
                var orderDetailProduct = new OrderDetail
                {
                    OrderId = 1,
                    OrderDetailId = 1,
                    ProductId = 1,
                    Quantity = 1,
                    UnitPrice = 15,
                    Product = product,
                };
                var orderDetailMeal = new OrderDetail
                {
                    OrderId = 1,
                    OrderDetailId = 2,
                    MealId = 1,
                    Quantity = 1,
                    UnitPrice = 25,
                    Meal = meal,
                };

                List<OrderDetail> orderDetails = new List<OrderDetail> { orderDetailProduct, orderDetailMeal };
                var o = new OrderHistoryViewModel
                {
                    Order = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderId = 1,
                        ShipDate = DateTime.Now,
                        ShippingAddress = new ShippingAddress
                        {
                            City = "HCM",
                            District = "Quan 9",
                            FullName = "Bao",
                            PhoneNumber = "1234567890",
                            ShippingAddressId = 1,
                            StreetAddress = "alo",
                            Ward = "truong thanh"
                        },
                        ShippingAddressId = 1,
                        Status = OrderStatus.Pending,
                        TotalPrice = 40,
                        UserId = (int)userId,
                        OrderDetails = orderDetails

                    },
                    AllReview = false,
                };
                var o1 = new OrderHistoryViewModel
                {
                    Order = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderId = 1,
                        ShipDate = DateTime.Now,
                        ShippingAddress = new ShippingAddress
                        {
                            City = "HCM",
                            District = "Quan 9",
                            FullName = "Bao",
                            PhoneNumber = "1234567890",
                            ShippingAddressId = 1,
                            StreetAddress = "alo",
                            Ward = "truong thanh"
                        },
                        ShippingAddressId = 1,
                        Status = OrderStatus.Completed,
                        TotalPrice = 40,
                        UserId = (int)userId,
                        OrderDetails = orderDetails

                    },
                    AllReview = true,
                };
                listOrder.Add(o);
                listOrder.Add(o1);
            }

            return View(listOrder);
        }
    }
}
