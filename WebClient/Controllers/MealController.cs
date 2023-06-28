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
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            HttpResponseMessage responseB = await client.GetAsync(BirdApiUrl);
            string strDataB = await responseB.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Product>? products = JsonSerializer.Deserialize<List<Product>>(strData, options);
            List<Bird>? birds = JsonSerializer.Deserialize<List<Bird>>(strDataB, options);
            ViewBag.Products = new SelectList(products, "ProductId", "ProductName");
            ViewBag.BirdMeals = new SelectList(birds, "BirdId", "BirdName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMealDTO createMealDTO, IFormFile mealImage)
        {
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

            HttpResponseMessage respone = await client.DeleteAsync(MealApiUrl + "/" + mealId);
            if (respone.IsSuccessStatusCode)
            {
                TempData["SuccMessage"] = "Delete Successfull!";
                return RedirectToAction("Index", "Meal");
            }
            TempData["ErrMessage"] = "Delete Failed!";
            return View();
        }
    }
}
