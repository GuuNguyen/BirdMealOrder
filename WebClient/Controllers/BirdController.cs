using BusinessObject.Models;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
    public class BirdController : Controller
    {
        private readonly HttpClient client;
        private string BirdApiUrl = "";

        public BirdController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BirdApiUrl = "https://localhost:7022/api/Bird";
        }
        public ActionResult Create()
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Bird bird, IFormFile birdImage)
        {
            if (birdImage != null && birdImage.Length > 0)
            {
                var task = new FirebaseStorage("groupprojectprn.appspot.com")
            .Child("bird-images")
            .Child(Path.GetFileName(birdImage.FileName))
            .PutAsync(birdImage.OpenReadStream());

                // Chờ upload hoàn thành
                var imageUrl = await task;

                // Lưu URL vào database
                bird.BirdImage = imageUrl;
            }
            string strData = JsonSerializer.Serialize(bird);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(BirdApiUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                TempData["msg"] = "Create successfully!";
            }
            else
            {
                TempData["msg"] = "Something Went Wrong!";
            }
            return RedirectToAction("Create", "Bird");
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("role") != "Staff")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(BirdApiUrl + "/" + id.ToString());
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Bird bird = JsonSerializer.Deserialize<Bird>(strData, options);
            return View(bird);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Bird bird, IFormFile birdImage)
        {
            if (birdImage != null && birdImage.Length > 0)
            {
                var task = new FirebaseStorage("groupprojectprn.appspot.com")
            .Child("bird-images")
            .Child(Path.GetFileName(birdImage.FileName))
            .PutAsync(birdImage.OpenReadStream());

                // Chờ upload hoàn thành
                var imageUrl = await task;

                // Lưu URL vào database
                bird.BirdImage = imageUrl;
            }
            string strData = JsonSerializer.Serialize(bird);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(BirdApiUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                TempData["msg"] = "Update successfully!";
            }
            else
            {
                TempData["msg"] = "Something Went Wrong!";
            }
            return RedirectToAction("Bird_Index", "Staff");
        }
    }
}
