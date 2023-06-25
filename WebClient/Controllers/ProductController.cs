using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        // GET: ProductController
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
        public async Task<ActionResult> Create(Product product)
        {
            string strData = JsonSerializer.Serialize(product);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ProductApiUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Create successfully";
            }
            else
            {
                ViewBag.Message = "Fail to call API";
            }
            return RedirectToAction("Index");
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
            var productStatusValues = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>().ToList();
            ViewBag.ProductStatus = new SelectList(productStatusValues);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Product product)
        {
            string strData = JsonSerializer.Serialize(product);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ProductApiUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Insert successfully!";
            }
            else
            {
                ViewBag.Message = "Error while calling WebAPI!";
            }
            return View(product);
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
