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
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl + "/GetOrdersAndCheckHasReviewByUserId/" + userId);

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


            return View(listOrder.OrderByDescending(o => o.Order.OrderId));
        }

        public async Task<IActionResult> OrderDetail(int OrderId)
        {
            var userId = HttpContext.Session.GetInt32("userID");
            if (userId == null)
            {
                return Redirect("/Login/Login");
            }
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl + "/GetOrdersAndCheckHasReviewByUserId/" + userId);

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
            var orderDetail = listOrder.SingleOrDefault(od => od.Order.OrderId == OrderId);
            return View(orderDetail);
        }
    }
}
