using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repositories.DTOs.SortDTO;
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
        private string OrderAPIUrl = "";
        private string FeedbackUrl = "";
        private string SearchUrl = "";
        private string SortAPIUrl = "";

        public HomeController()
        {     
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            MealAPIUrl = "https://localhost:7022/api/Meal";
            BirdAPIUrl = "https://localhost:7022/api/Bird";
            ProductUrl = "https://localhost:7022/api/Product";
            OrderAPIUrl = "https://localhost:7022/api/Order";
            FeedbackUrl = "https://localhost:7022/api/Feedback";
            SearchUrl = "https://localhost:7022/api/Search";
            SortAPIUrl = "https://localhost:7022/api/Sort";
        }

        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("role");
            if (!string.IsNullOrEmpty(role))
            {
                if(role == "Admin") return RedirectToAction("User_Index", "Staff");
                else if(role == "Staff") return RedirectToAction("Product_Index", "Staff");
            }

            var mealTask = _client.GetAsync(MealAPIUrl);
            var birdTask = _client.GetAsync(BirdAPIUrl);
            var bestSellerTask = _client.GetAsync(OrderAPIUrl + "/GetBestSeller");
            var recommendTask = _client.GetAsync(OrderAPIUrl + "/RecommendList");

            await Task.WhenAll(mealTask, birdTask, bestSellerTask, recommendTask);

            var mealResponse = await mealTask;
            var birdResponse = await birdTask;
            var bestSellerResponse = await bestSellerTask;
            var recommendResponse = await recommendTask;

            string mealStrData = await mealResponse.Content.ReadAsStringAsync();
            string birdStrData = await birdResponse.Content.ReadAsStringAsync();
            string bestSellerData = await bestSellerResponse.Content.ReadAsStringAsync();
            string recommendData = await recommendResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var listMeal = JsonSerializer.Deserialize<List<Meal>>(mealStrData, options);
            var listBird = JsonSerializer.Deserialize<List<Bird>>(birdStrData, options);
            var listBestSellers = JsonSerializer.Deserialize<List<object>>(bestSellerData, options);
            var listRecommends = JsonSerializer.Deserialize<List<object>>(recommendData, options);

            var finalList = new HomeViewModel
            {
                Meals = listMeal ?? new List<Meal>(),
                Birds = listBird ?? new List<Bird>(),
                BestSellers = listBestSellers ?? new List<object>(),
                HighlyRecommends = listRecommends ?? new List<object>(),
            };
            return View(finalList);
        }

        public async Task<IActionResult> Meal()
        {
            var role = HttpContext.Session.GetString("role");
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Admin") return RedirectToAction("User_Index", "Staff");
                else if (role == "Staff") return RedirectToAction("Product_Index", "Staff");
            }

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
                Meals = listMeal ?? new List<Meal>(),
                Birds = listBird ?? new List<Bird>(),
                PageType = "Meal"
            };
            return View(finalResult);
        }

        public async Task<IActionResult> MealsByBirdId(int id)
        {
            var role = HttpContext.Session.GetString("role");
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Admin") return RedirectToAction("User_Index", "Staff");
                else if (role == "Staff") return RedirectToAction("Product_Index", "Staff");
            }

            HttpResponseMessage mealResponse = await _client.GetAsync(MealAPIUrl + $"/ByBirdId/{id}");
            HttpResponseMessage birdResponse = await _client.GetAsync(BirdAPIUrl);
            HttpResponseMessage birdSpecificResponse = await _client.GetAsync(BirdAPIUrl + $"/{id}");
            string mealStrData = await mealResponse.Content.ReadAsStringAsync();
            string birdStrData = await birdResponse.Content.ReadAsStringAsync();
            string birdSpecificStrData = await birdSpecificResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Meal> listMeal = new List<Meal>();
            listMeal = JsonSerializer.Deserialize<List<Meal>>(mealStrData, options);

            List<Bird> listBird = new List<Bird>();
            listBird = JsonSerializer.Deserialize<List<Bird>>(birdStrData, options);

            Bird birdSpecific = new Bird();
            birdSpecific = JsonSerializer.Deserialize<Bird>(birdSpecificStrData, options);

            var breadcrumbs = new List<BreadCrumb>
            {
                new BreadCrumb { Text = "Home", Url = "/" },
                new BreadCrumb { Text = "Bird", Url = "/Home/Bird" },
                new BreadCrumb { Text = birdSpecific.BirdName, Url = "#" },
                new BreadCrumb { Text = "Meal", Url = "/Home/Meal" },
            };
            var finalResult = new MealViewModel
            {
                Breadcrumbs = breadcrumbs,
                Meals = listMeal ?? new List<Meal>(),
                Birds = listBird ?? new List<Bird>(),
                PageType = "Meal"
            };
            return View("Meal", finalResult);
        }

        public async Task<IActionResult> Food()
        {
            var role = HttpContext.Session.GetString("role");
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Admin") return RedirectToAction("User_Index", "Staff");
                else if (role == "Staff") return RedirectToAction("Product_Index", "Staff");
            }

            HttpResponseMessage productResponse = await _client.GetAsync(ProductUrl);

            string productStrData = await productResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Product> listProduct = new List<Product>();
            listProduct = JsonSerializer.Deserialize<List<Product>>(productStrData, options);

            var breadcrumbs = new List<BreadCrumb>
            {
                new BreadCrumb { Text = "Home", Url = "/" },
                new BreadCrumb { Text = "Food", Url = "/Home/Food" }
            };
            var finalResult = new FoodViewModel
            {
                Breadcrumbs = breadcrumbs,
                Products = listProduct ?? new List<Product>(),
                Birds = null,
                PageType = "Food"
            };
            return View(finalResult);
        }

        public async Task<IActionResult> Detail(string code)
        {
            var role = HttpContext.Session.GetString("role");
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Admin") return RedirectToAction("User_Index", "Staff");
                else if (role == "Staff") return RedirectToAction("Product_Index", "Staff");
            }

            if (!string.IsNullOrEmpty(code))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Meal? meal = null;
                Product? product = null;
                List<Meal>? listMeal = null;
                List<Product>? listProduct = null;
                List<Bird>? listBird = new List<Bird>();
                List<Product>? productIngredients = null;
                List<MealProduct>? listMealProduct = null;
                ListFeedbackViewModel? listFeedbackViewModel = null;
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


                        HttpResponseMessage birdResponse = await _client.GetAsync(BirdAPIUrl + $"/ByMeal/{meal.MealId}");
                        HttpResponseMessage productIngredientsResponse = await _client.GetAsync(ProductUrl + $"/ByMeal/{meal.MealId}");

                        string birdStrData = await birdResponse.Content.ReadAsStringAsync();
                        string productIngredientsStrData = await productIngredientsResponse.Content.ReadAsStringAsync();
                        productIngredients = JsonSerializer.Deserialize<List<Product>>(productIngredientsStrData, options);
                        listBird = JsonSerializer.Deserialize<List<Bird>>(birdStrData, options);
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
                        HttpResponseMessage allBirdResponse = await _client.GetAsync(BirdAPIUrl);

                        string allBirdStrData = await allBirdResponse.Content.ReadAsStringAsync();
                        string listProductStrData = await listProductResponse.Content.ReadAsStringAsync();
                        string productStrData = await productResponse.Content.ReadAsStringAsync();

                        listBird = JsonSerializer.Deserialize<List<Bird>>(allBirdStrData, options);
                        listProduct = JsonSerializer.Deserialize<List<Product>>(listProductStrData, options);
                        product = JsonSerializer.Deserialize<Product>(productStrData, options);
                        var foodBreadCrumb = new BreadCrumb { Text = "Food", Url = "/Home/Food" };
                        var foodBreadCrumbDetail = new BreadCrumb { Text = product.ProductName, Url = $"/Home/Detail?code={product.ProductCode}" };
                        breadcrumbs.Add(foodBreadCrumb);
                        breadcrumbs.Add(foodBreadCrumbDetail);

                        HttpResponseMessage feedbackProductResponse = await _client.GetAsync(FeedbackUrl + $"/GetListFeedbackByProductId/{product.ProductId}");
                        string feedbackProductStrData = await feedbackProductResponse.Content.ReadAsStringAsync();
                        listFeedbackViewModel = JsonSerializer.Deserialize<ListFeedbackViewModel>(feedbackProductStrData, options);
                        break;
                }
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

        public async Task<IActionResult> Bird()
        {
            var role = HttpContext.Session.GetString("role");
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Admin") return RedirectToAction("User_Index", "Staff");
                else if (role == "Staff") return RedirectToAction("Product_Index", "Staff");
            }

            HttpResponseMessage response = await _client.GetAsync(BirdAPIUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Bird> listBird = JsonSerializer.Deserialize<List<Bird>>(strData, options);

            var breadcrumbs = new List<BreadCrumb>
            {
                new BreadCrumb { Text = "Home", Url = "/" },
                new BreadCrumb { Text = "Bird", Url = "/Home/Bird" }
            };
            var finalResult = new BirdViewModel
            {
                Breadcrumbs = breadcrumbs,
                Birds = listBird ?? new List<Bird>(),
            };
            return View(finalResult);
        }

        public async Task<IActionResult> Search(string keyWord)
        {
            HttpResponseMessage response = await _client.GetAsync(SearchUrl + "/" + keyWord);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            SearchViewModel result = JsonSerializer.Deserialize<SearchViewModel>(strData, options);
            return View(result);
        }

        public async Task<IActionResult> ApplyFilter(SortInputDTO value)
        {
            var sortValue = JsonSerializer.Serialize(value);
            var contentData = new StringContent(sortValue, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(SortAPIUrl, contentData);
            string strData = "";
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            if (response.IsSuccessStatusCode)
            {
                strData = await response.Content.ReadAsStringAsync();
                if(strData == "[]") return NotFound();
                if (value.PageType == "Meal")
                {
                    List<Meal> listMeal = JsonSerializer.Deserialize<List<Meal>>(strData, options);
                    return Json(listMeal);
                }
                else
                {
                    List<Product> listProduct = JsonSerializer.Deserialize<List<Product>>(strData, options);;
                    return Json(listProduct);
                }                
            }
            return BadRequest();
        }
    }
}
