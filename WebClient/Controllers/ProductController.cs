using BusinessObject.Enums;
using BusinessObject.Models;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories.DTOs.ProductDTO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient client;
        private string ProductApiUrl = "";

        public ProductController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7022/api/Product";
        }

        public async Task<ActionResult> Index()
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

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product, IFormFile productImage)
        {
            if (productImage != null && productImage.Length > 0)
            {
                var task = new FirebaseStorage("groupprojectprn.appspot.com")
            .Child("product-images")
            .Child(Path.GetFileName(productImage.FileName))
            .PutAsync(productImage.OpenReadStream());

                // Chờ upload hoàn thành
                var imageUrl = await task;

                // Lưu URL vào database
                product.ProductImage = imageUrl;
            }
            string strData = JsonSerializer.Serialize(product);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ProductApiUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                TempData["msg"] = "Create successfully!";
            }
            else
            {
                TempData["msg"] = "Something Went Wrong!";
            }
            return RedirectToAction("Create", "Product");
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + "/" + id.ToString());
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Product product = JsonSerializer.Deserialize<Product>(strData, options);
            var productStatusValues = Enum.GetValues(typeof(ProductStatus));
            ViewBag.ProductStatus = new SelectList(productStatusValues, product.ProductStatus);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductDTO product, IFormFile pImage)
        {
            var oldFile = Path.GetFileName(product.ProductImage);

            var startIndex = oldFile.IndexOf("product-images%2F") + "product-images%2F".Length;
            var endIndex = oldFile.IndexOf("?alt=media&token=");

            var cleanFileName = oldFile.Substring(startIndex, endIndex - startIndex);

            if (pImage != null && pImage.Length > 0)
            {
                var newFileName = Path.GetFileName(pImage.FileName);

                var task = new FirebaseStorage("groupprojectprn.appspot.com")
                    .Child("meal-images")
                    .Child(newFileName)
                    .PutAsync(pImage.OpenReadStream());

                // Chờ upload hoàn thành
                var newImageUrl = await task;

                // Lưu URL mới vào updateMealDTO
                product.ProductImage = newImageUrl;
            }

            string strData = JsonSerializer.Serialize(product);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(ProductApiUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                if (pImage != null && pImage.Length > 0)
                {
                    await new FirebaseStorage("groupprojectprn.appspot.com")
                        .Child("meal-images")
                        .Child(cleanFileName)
                        .DeleteAsync();
                }
                TempData["msg"] = "Update successfully!";
            }
            else
            {
                TempData["msg"] = "Something Went Wrong!";
            }
            return RedirectToAction("Product_Index", "Staff");
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + "/" + id);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Product product = JsonSerializer.Deserialize<Product>(strData, options);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Product product)
        {
            HttpResponseMessage response = await client.DeleteAsync(ProductApiUrl + "/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Edit successfully!";
            }
            else
            {
                ViewBag.Message = "Error while calling WebAPI!";
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + "/" + id);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Product product = JsonSerializer.Deserialize<Product>(strData, options);
            return View(product);
        }
    }
}
