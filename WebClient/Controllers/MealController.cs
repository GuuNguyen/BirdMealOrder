using Azure;
using BusinessObject.Enums;
using BusinessObject.Models;
using DataAccess.DAOs;
using Firebase.Storage;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Repositories.DTOs.MealDTO;
using System.Net;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text.Json;
using WebClient.ViewModels;

namespace WebClient.Controllers
{
    public class MealController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        private string BirdApiUrl = "";
        private string MealApiUrl = "";
        public MealController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7022/api/Product";
            BirdApiUrl = "https://localhost:7022/api/Bird";
            MealApiUrl = "https://localhost:7022/api/Meal";
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            HttpResponseMessage responseB = await client.GetAsync(BirdApiUrl);
            string strDataB = await responseB.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Product>? products = JsonSerializer.Deserialize<List<Product>>(strData, options);
            var availableProducts = products.Where(p => p.ProductStatus == ProductStatus.Available).ToList();
            List<Bird>? birds = JsonSerializer.Deserialize<List<Bird>>(strDataB, options);
            ViewBag.Products = new SelectList(availableProducts, "ProductId", "ProductName");
            ViewBag.BirdMeals = new SelectList(birds, "BirdId", "BirdName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMealDTO createMealDTO, IFormFile mealImage)
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var productIds = new List<int>();
            foreach (var product in createMealDTO.ProductOptions)
            {
                if (productIds.Contains(product.ProductId))
                {
                    TempData["ErrMessage"] = "You cannot create a meal from two same products!";
                    return RedirectToAction("Create");
                }
                else
                {
                    productIds.Add(product.ProductId); // Thêm ID vào danh sách đã xuất hiện
                }
            }
            if (mealImage != null && mealImage.Length > 0)
            {
                var task = new FirebaseStorage("groupprojectprn.appspot.com")
            .Child("meal-images")
            .Child(Path.GetFileName(mealImage.FileName))
            .PutAsync(mealImage.OpenReadStream());

                // Chờ upload hoàn thành
                var imageUrl = await task;

                // Lưu URL vào database
                createMealDTO.MealImage = imageUrl;
            }
            string jsonStr = JsonSerializer.Serialize(createMealDTO);
            var contentData = new StringContent(jsonStr, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(MealApiUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccMessage"] = "Create Successfull!";
                return RedirectToAction("Create");
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrMessage"] = errorMessage;
            }
            else
            {
                TempData["ErrMessage"] = "Create Failed!";
            }
            return RedirectToAction("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int mealId)
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respone = await client.DeleteAsync(MealApiUrl + "/" + mealId);
            if (respone.IsSuccessStatusCode)
            {
                return RedirectToAction("Meal_Index", "Staff");
            }
            return RedirectToAction("Meal_Index", "Staff");
        }

        public async Task<IActionResult> Details(int mealId)
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respone = await client.GetAsync(MealApiUrl + "/" + mealId);
            string strData = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Meal? meal = JsonSerializer.Deserialize<Meal>(strData, options);
            return View(meal);
        }

        public async Task<IActionResult> Edit(int mealId)
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respone = await client.GetAsync(MealApiUrl + "/GetMealIncludeBird&Product/" + mealId);
            string strData = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            UpdateMealDTO? meal = JsonSerializer.Deserialize<UpdateMealDTO>(strData, options);
            var statusList = Enum.GetValues(typeof(MealStatus));
            ViewBag.MealStatus = new SelectList(statusList, meal.MealStatus);


            HttpResponseMessage responseP = await client.GetAsync(ProductApiUrl);
            string strDataP = await responseP.Content.ReadAsStringAsync();
            HttpResponseMessage responseB = await client.GetAsync(BirdApiUrl);
            string strDataB = await responseB.Content.ReadAsStringAsync();

            List<Product>? products = JsonSerializer.Deserialize<List<Product>>(strDataP, options);
            List<Bird>? birds = JsonSerializer.Deserialize<List<Bird>>(strDataB, options);

            ViewBag.Products = new SelectList(products, "ProductId", "ProductName");
            ViewBag.BirdMeals = new SelectList(birds, "BirdId", "BirdName");

            return View(meal);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(UpdateMealDTO updateMealDTO, IFormFile mImage)
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var productIds = new List<int>();
            foreach (var product in updateMealDTO.ProductRequireds)
            {
                if (productIds.Contains(product.ProductId))
                {
                    TempData["ErrMessage"] = "You cannot create a meal from two same products!";
                    return RedirectToAction("Create");
                }
                else
                {
                    productIds.Add(product.ProductId); // Thêm ID vào danh sách đã xuất hiện
                }
            }
            // Lấy tên tệp tin cũ
            var oldFile = Path.GetFileName(updateMealDTO.MealImage);

            var startIndex = oldFile.IndexOf("meal-images%2F") + "meal-images%2F".Length;
            var endIndex = oldFile.IndexOf("?alt=media&token=");

            var cleanFileName = oldFile.Substring(startIndex, endIndex - startIndex);

            if (mImage != null && mImage.Length > 0)
            {
                var newFileName = Path.GetFileName(mImage.FileName);

                var task = new FirebaseStorage("groupprojectprn.appspot.com")
                    .Child("meal-images")
                    .Child(newFileName)
                    .PutAsync(mImage.OpenReadStream());

                // Chờ upload hoàn thành
                var newImageUrl = await task;

                // Lưu URL mới vào updateMealDTO
                updateMealDTO.MealImage = newImageUrl;
            }

            string jsonStr = JsonSerializer.Serialize(updateMealDTO);
            var contentData = new StringContent(jsonStr, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(MealApiUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                // Xóa tệp tin cũ
                if (mImage != null && mImage.Length > 0)
                {
                    await new FirebaseStorage("groupprojectprn.appspot.com")
                    .Child("meal-images")
                    .Child(cleanFileName)
                    .DeleteAsync();
                }
                TempData["SuccMessage"] = "Update Successfull!";
                return RedirectToAction("Meal_Index", "Staff");
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrMessage"] = errorMessage;
            }
            else
            {
                TempData["ErrMessage"] = "Update Failed!";
            }


            return RedirectToAction("Meal_Index", "Staff");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string code)
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
                string prefix = code.Substring(0, 2);
                switch (prefix)
                {
                    case "ME":
                        HttpResponseMessage listMealResponse = await client.GetAsync(MealApiUrl);
                        HttpResponseMessage mealResponse = await client.GetAsync(MealApiUrl + $"/Detail/{code}");

                        string listMealStrData = await listMealResponse.Content.ReadAsStringAsync();
                        string mealStrData = await mealResponse.Content.ReadAsStringAsync();

                        listMeal = JsonSerializer.Deserialize<List<Meal>>(listMealStrData, options);
                        meal = JsonSerializer.Deserialize<Meal>(mealStrData, options);

                        HttpResponseMessage productIngredientsResponse = await client.GetAsync(ProductApiUrl + $"/ByMeal/{meal.MealId}");
                        string productIngredientsStrData = await productIngredientsResponse.Content.ReadAsStringAsync();
                        productIngredients = JsonSerializer.Deserialize<List<Product>>(productIngredientsStrData, options);

                        var mealBreadCrumb = new BreadCrumb { Text = "Meal", Url = "/Home/Meal" };
                        var mealBreadCrumbDetail = new BreadCrumb { Text = meal.MealName, Url = $"/Home/Detail?code={meal.MealCode}" };
                        break;
                }
                HttpResponseMessage birdResponse = await client.GetAsync(BirdApiUrl);
                string birdStrData = await birdResponse.Content.ReadAsStringAsync();
                listBird = JsonSerializer.Deserialize<List<Bird>>(birdStrData, options);

                HttpResponseMessage mealProductResponse = await client.GetAsync(MealApiUrl + $"/GetMealProductByMealId/{meal.MealId}");
                string mealProductStrData = await mealProductResponse.Content.ReadAsStringAsync();
                var listMealProduct = JsonSerializer.Deserialize<List<MealProduct>>(mealProductStrData, options);
                var finalResult = new DetailPMViewModel
                {
                    Meal = meal,
                    Product = product,
                    Meals = listMeal,
                    ProductIngredients = productIngredients,
                    Birds = listBird,
                    Products = listProduct,
                    MealProducts = listMealProduct
                };
                return View(finalResult);
            }
            return View();
        }
    }
}
