using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
    public class StaffController : Controller
    {
        private readonly HttpClient client;
        private string ProductApiUrl = "";
        private string UserApiUrl = "";
        private string MealApiUrl = "";
        private string BirdApiUrl = "";

        public StaffController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7022/api/Product";
            UserApiUrl = "https://localhost:7022/api/User";
            MealApiUrl = "https://localhost:7022/api/Meal";
            BirdApiUrl = "https://localhost:7022/api/Bird";
        }
        public async Task<IActionResult> Product_Index()
        {
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
            HttpResponseMessage response = await client.GetAsync(BirdApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Bird> listBird = JsonSerializer.Deserialize<List<Bird>>(strData, options);
            return View(listBird);
        }
    }
}
