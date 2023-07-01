using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

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
    }
}
