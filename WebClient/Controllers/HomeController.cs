using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        private string ProductUrl = "";
        private string FeedbackUrl = "";

        public HomeController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            MealAPIUrl = "https://localhost:7022/api/Meal";
            BirdAPIUrl = "https://localhost:7022/api/Bird";
            ProductUrl = "https://localhost:7022/api/Product";
            FeedbackUrl = "https://localhost:7022/api/Feedback";
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
            HttpResponseMessage birdResponse = await _client.GetAsync(BirdAPIUrl);
            string mealStrData = await mealResponse.Content.ReadAsStringAsync();
            string birdStrData = await birdResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Meal> listMeal = new List<Meal>();
            listMeal = JsonSerializer.Deserialize<List<Meal>>(mealStrData, options);

            List<Bird> listBird = new List<Bird>();
            listBird = JsonSerializer.Deserialize<List<Bird>>(birdStrData, options);

            var breadcrumbs = new List<BreadCrumb>
            {
                new BreadCrumb { Text = "Home", Url = "/" },
                new BreadCrumb { Text = "Meal", Url = "/Home/Meal" }
            };
            var finalResult = new MealViewModel
            {
                Breadcrumbs = breadcrumbs,
                Meals = listMeal,
                Birds = listBird
            };
            return View(finalResult);
        }

        public async Task<IActionResult> Food()
        {
            HttpResponseMessage productResponse = await _client.GetAsync(ProductUrl);
            HttpResponseMessage birdResponse = await _client.GetAsync(BirdAPIUrl);
            string productStrData = await productResponse.Content.ReadAsStringAsync();
            string birdStrData = await birdResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Product> listProduct = new List<Product>();
            listProduct = JsonSerializer.Deserialize<List<Product>>(productStrData, options);

            List<Bird> listBird = new List<Bird>();
            listBird = JsonSerializer.Deserialize<List<Bird>>(birdStrData, options);

            var breadcrumbs = new List<BreadCrumb>
            {
                new BreadCrumb { Text = "Home", Url = "/" },
                new BreadCrumb { Text = "Food", Url = "/Home/Food" }
            };
            var finalResult = new FoodViewModel
            {
                Breadcrumbs = breadcrumbs,
                Products = listProduct,
                Birds = listBird
            };
            return View(finalResult);
        }

        public async Task<IActionResult> Detail(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Meal meal = null;
                Product product = null;
                List<Meal> listMeal = null;
                List<Product> listProduct = null;
                List<Bird> listBird = new List<Bird>();
                List<Product> productIngredients = null;
                List<MealProduct> listMealProduct = null;
                ListFeedbackViewModel listFeedbackViewModel = null;
                var breadcrumbs = new List<BreadCrumb>
                {
                    new BreadCrumb { Text = "Home", Url = "/" },
                };
                string prefix = code.Substring(0, 2);
                switch (prefix)
                {
                    case "ME":
                        HttpResponseMessage listMealResponse = await _client.GetAsync(MealAPIUrl);
                        HttpResponseMessage mealResponse = await _client.GetAsync(MealAPIUrl + $"/Detail/{code}");

                        string listMealStrData = await listMealResponse.Content.ReadAsStringAsync();
                        string mealStrData = await mealResponse.Content.ReadAsStringAsync();

                        listMeal = JsonSerializer.Deserialize<List<Meal>>(listMealStrData, options);
                        meal = JsonSerializer.Deserialize<Meal>(mealStrData, options);

                        HttpResponseMessage productIngredientsResponse = await _client.GetAsync(ProductUrl + $"/ByMeal/{meal.MealId}");
                        string productIngredientsStrData = await productIngredientsResponse.Content.ReadAsStringAsync();
                        productIngredients = JsonSerializer.Deserialize<List<Product>>(productIngredientsStrData, options);

                        var mealBreadCrumb = new BreadCrumb { Text = "Meal", Url = "/Home/Meal" };
                        var mealBreadCrumbDetail = new BreadCrumb { Text = meal.MealName, Url = $"/Home/Detail?code={meal.MealCode}" };

                        breadcrumbs.Add(mealBreadCrumb);
                        breadcrumbs.Add(mealBreadCrumbDetail);
                        HttpResponseMessage mealProductResponse = await _client.GetAsync(MealAPIUrl + $"/GetMealProductByMealId/{meal.MealId}");
                        string mealProductStrData = await mealProductResponse.Content.ReadAsStringAsync();
                        listMealProduct = JsonSerializer.Deserialize<List<MealProduct>>(mealProductStrData, options);

                        HttpResponseMessage feedbackMealResponse = await _client.GetAsync(FeedbackUrl + $"/GetListFeedbackByMeald/{meal.MealId}");
                        string feedbackMealStrData = await feedbackMealResponse.Content.ReadAsStringAsync();
                        listFeedbackViewModel = JsonSerializer.Deserialize<ListFeedbackViewModel>(feedbackMealStrData, options);
                        break;
                    case "FO":
                        HttpResponseMessage listProductResponse = await _client.GetAsync(ProductUrl);
                        HttpResponseMessage productResponse = await _client.GetAsync(ProductUrl + $"/Detail/{code}");
                        string listProductStrData = await listProductResponse.Content.ReadAsStringAsync();
                        string productStrData = await productResponse.Content.ReadAsStringAsync();
                        listProduct = JsonSerializer.Deserialize<List<Product>>(listProductStrData, options);
                        product = JsonSerializer.Deserialize<Product>(productStrData, options);
                        var foodBreadCrumb = new BreadCrumb { Text = "Food", Url = "/Home/Food" };
                        var foodBreadCrumbDetail = new BreadCrumb { Text = product.ProductName, Url = $"/Home/Detail?code={product.ProductCode}" };
                        breadcrumbs.Add(foodBreadCrumb);
                        breadcrumbs.Add(foodBreadCrumbDetail);

                        HttpResponseMessage feedbackProductResponse = await _client.GetAsync(FeedbackUrl + $"/GetListFeedbackByMeald/{product.ProductId}");
                        string feedbackProductStrData = await feedbackProductResponse.Content.ReadAsStringAsync();
                        listFeedbackViewModel = JsonSerializer.Deserialize<ListFeedbackViewModel>(feedbackProductStrData, options);
                        break;
                }
                HttpResponseMessage birdResponse = await _client.GetAsync(BirdAPIUrl);
                string birdStrData = await birdResponse.Content.ReadAsStringAsync();
                listBird = JsonSerializer.Deserialize<List<Bird>>(birdStrData, options);
                var finalResult = new DetailPMViewModel
                {
                    Breadcrumbs = breadcrumbs,
                    Meal = meal,
                    Product = product,
                    Meals = listMeal,
                    ProductIngredients = productIngredients,
                    Birds = listBird,
                    Products = listProduct,
                    MealProducts = listMealProduct ?? new List<MealProduct>(),
                    ListFeedbackViewModel = listFeedbackViewModel,
                };
                return View(finalResult);
            }
            return View();
        }
    }
}
