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
        private string OrderDetailApiUrl = "";

        public OrderController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "https://localhost:7022/api/Order";
            OrderDetailApiUrl = "https://localhost:7022/api/OrderDetail";
        }

        [HttpGet]
        public async Task<IActionResult> OrderHistory()
        {
            if (HttpContext.Session.GetString("role") != "Customer")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
            if (HttpContext.Session.GetString("role") != "Customer")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

        public async Task<IActionResult> ListOrderDetail(int id)
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl + "/" + id);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Order? order = JsonSerializer.Deserialize<Order>(strData, options);
            return View(order);
        }
    }
}
