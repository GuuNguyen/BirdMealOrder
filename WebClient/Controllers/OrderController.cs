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
                        TotalPrice = 0,
                        UserId = (int)userId,

                    }
                };
                listOrder.Add(o);
            }

            return View(listOrder);
        }
    }
}
