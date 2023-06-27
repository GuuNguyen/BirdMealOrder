using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
    public class MealController : Controller
    {
        private readonly HttpClient client;
        private string MealtApiUrl = "";

        public MealController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MealtApiUrl = "https://localhost:7022/api/Meal";
        }

        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(MealtApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Meal> listProduct = JsonSerializer.Deserialize<List<Meal>>(strData, options);
            return View(listProduct);
        }
    }
}
