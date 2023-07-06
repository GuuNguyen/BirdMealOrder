using BusinessObject.Enums;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories.DTOs.OrderDTO;
using System.Net.Http.Headers;
using System.Text.Json;
using WebClient.ViewModels;

namespace WebClient.Controllers
{
    public class StaffController : Controller
    {
        private readonly HttpClient client;
        private string ProductApiUrl = "";
        private string UserApiUrl = "";
        private string MealApiUrl = "";
        private string BirdApiUrl = "";
        private string OrderApiUrl = "";
        private string OrderDetailApiUrl = "";

        public StaffController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7022/api/Product";
            UserApiUrl = "https://localhost:7022/api/User";
            MealApiUrl = "https://localhost:7022/api/Meal";
            BirdApiUrl = "https://localhost:7022/api/Bird";
            OrderApiUrl = "https://localhost:7022/api/Order";
            OrderDetailApiUrl = "https://localhost:7022/api/OrderDetail";
        }
        public async Task<IActionResult> Product_Index()
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Product> listProduct = JsonSerializer.Deserialize<List<Product>>(strData, options);

            return View(listProduct);
        }

        public async Task<IActionResult> User_Index()
        {
            if (HttpContext.Session.GetString("role") != "Admin")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<User> listUser = JsonSerializer.Deserialize<List<User>>(strData, options);
            return View(listUser);
        }

        public async Task<IActionResult> Meal_Index()
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(MealApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Meal> listMeal = JsonSerializer.Deserialize<List<Meal>>(strData, options);
            return View(listMeal);
        }
        public async Task<IActionResult> Bird_Index()
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(BirdApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Bird> listBird = JsonSerializer.Deserialize<List<Bird>>(strData, options);
            return View(listBird);
        }

        public async Task<IActionResult> Report_Index()
        {
            if (HttpContext.Session.GetString("role") != "Admin")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(OrderDetailApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<SalesReportViewModel> salesReportViewModels = JsonSerializer.Deserialize<List<SalesReportViewModel>>(strData, options);
            return View(salesReportViewModels.Where(s => s.Order.Status != BusinessObject.Enums.OrderStatus.Pending && s.Order.Status != BusinessObject.Enums.OrderStatus.Processing).ToList());
        }

        public async Task<IActionResult> Order_Index()
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Order> listOrder = JsonSerializer.Deserialize<List<Order>>(strData, options);

            ViewBag.OrderStatus = GetOrderStatusSelectList();
            ViewBag.OrderStatusNoneCompleted = GetOrderStatusSelectListNoneComppleted();

            return View(listOrder);
        }

        private SelectList GetOrderStatusSelectList()
        {
            var orderStatuses = Enum.GetValues(typeof(OrderStatus));
            return new SelectList(orderStatuses);
        }
        private SelectList GetOrderStatusSelectListNoneComppleted()
        {
            var orderStatuses = new List<OrderStatus> { OrderStatus.Pending, OrderStatus.Processing, OrderStatus.Canceled };
            return new SelectList(orderStatuses);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeOrderStatus(ChangeOrderStatusDTO order)
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string strData = JsonSerializer.Serialize(order);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(OrderApiUrl + "/ChangeOrderStatus", contentData);
            if (response.IsSuccessStatusCode)
            {
                TempData["msg"] = "Update successfully!";
            }
            else
            {
                TempData["msg"] = "Something Went Wrong!";
            }
            return RedirectToAction("Order_Index", "Staff");
        }
    }
}
