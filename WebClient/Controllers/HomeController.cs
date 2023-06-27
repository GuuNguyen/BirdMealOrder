using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using WebClient.ViewModels;
using WebClient.VIewModels;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private string MealAPIUrl = "";
        private string BirdAPIUrl = "";

        public HomeController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            MealAPIUrl = "https://localhost:7022/api/Meal";
            BirdAPIUrl = "https://localhost:7022/api/Bird";
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage mealResponse = await _client.GetAsync(MealAPIUrl);
            HttpResponseMessage birdResponse = await _client.GetAsync(BirdAPIUrl);
            string mealStrData = await mealResponse.Content.ReadAsStringAsync();
            string birdStrData = await birdResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Meal> listMeal = new List<Meal>();
            List<Bird> listBird = new List<Bird>();
            listMeal = JsonSerializer.Deserialize<List<Meal>>(mealStrData, options);
            listBird = JsonSerializer.Deserialize<List<Bird>>(birdStrData, options);

            var finalList = new HomeViewModel
            {
                Meals = listMeal,
                Birds = listBird
            };

            return View(finalList);
        }

        public async Task<IActionResult> Meal()
        {
            HttpResponseMessage mealResponse = await _client.GetAsync(MealAPIUrl);
            string mealStrData = await mealResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            List<Meal> listMeal = new List<Meal>();
            listMeal = JsonSerializer.Deserialize<List<Meal>>(mealStrData, options);

            var breadcrumbs = new List<BreadCrumb>
            {
                new BreadCrumb { Text = "Home", Url = "/" },
                new BreadCrumb { Text = "Meals", Url = "/Home/Meal" }
            };
            var finalResult = new MealViewModel
            {
                Breadcrumbs = breadcrumbs,
                Meals = listMeal
            };
            return View(finalResult);
        }
    }
}
