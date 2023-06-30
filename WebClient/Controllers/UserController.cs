using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebClient.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;
        private string UserAPIUrl = "";
        private string RoleAPIUrl = "";

        public UserController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            UserAPIUrl = "https://localhost:7022/api/User";
            RoleAPIUrl = "https://localhost:7022/api/Role";
        }


        public async Task<IActionResult> Index(string SearchKey)
        {
            //if (HttpContext.Session.GetString("role") != "admin") return RedirectToAction("Login", "Login");
            HttpResponseMessage response = await _client.GetAsync(UserAPIUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<User>? list = JsonSerializer.Deserialize<List<User>>(strData, options);
            if (string.IsNullOrEmpty(SearchKey))
            {
                return View(list);
            }

            return View(list.Where(l => l.FullName.ToLower().Contains(SearchKey.ToLower()) || l.UserName.ToLower().Contains(SearchKey.ToLower()) ).ToList());
        }

        public async Task<IActionResult> CreateUser()
        {
            HttpResponseMessage responseRole = await _client.GetAsync(RoleAPIUrl);
            string strDataRole = await responseRole.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Role>? listRole = JsonSerializer.Deserialize<List<Role>>(strDataRole, options);
            ViewBag.RoleId = new SelectList(listRole, "RoleId", "RoleName");

            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateUser(User user)
        //{
        //    string jsonStr = JsonSerializer.Serialize(user);
        //    var contentData = new StringContent(jsonStr, System.Text.Encoding.UTF8, "application/json");
        //    HttpResponseMessage respone = await _client.PostAsync(UserAPIUrl, contentData);

        //    if (respone.IsSuccessStatusCode)
        //    {
                

        //        TempData["SuccMessage"] = "Create Successfull!";
        //        return RedirectToAction("CreateUser", "User");
        //    }
        //    ModelState.AddModelError(String.Empty, "Failed to call API");
        //    return RedirectToAction("CreateUser", "User");
        //}

        
        [HttpPost]
        public async Task<IActionResult> CreateUserFull(User user)
        {
            HttpResponseMessage responseRole = await _client.GetAsync(RoleAPIUrl);
            string strDataRole = await responseRole.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Role>? listRole = JsonSerializer.Deserialize<List<Role>>(strDataRole, options);
            ViewBag.RoleId = new SelectList(listRole, "RoleId", "RoleName");



            string jsonStr = JsonSerializer.Serialize(user);
            var contentData = new StringContent(jsonStr, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage respone = await _client.PostAsync(UserAPIUrl + "/" +"StaffCreateUser", contentData);

            if (respone.IsSuccessStatusCode)
            {
                TempData["SuccMessage"] = "Create Successfull!";
                return RedirectToAction("CreateUser", "User");
            }
            ModelState.AddModelError(String.Empty, "Failed to call API");
            return RedirectToAction("CreateUser", "User");
        }
        






        public async Task<IActionResult> DeleteUser(int id)
        {
            //if (HttpContext.Session.GetString("role") != "admin")
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            HttpResponseMessage response = await _client.GetAsync(UserAPIUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            User? user = JsonSerializer.Deserialize<User>(strData, options);
            return View(user);
            // return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string tmp, int id)
        {
            //if (HttpContext.Session.GetString("role") != "admin")
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            HttpResponseMessage response = await _client.DeleteAsync(UserAPIUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Delete successfully!";
                return RedirectToAction("Index", "User");
            }
            ModelState.AddModelError(String.Empty, "Failed to call API!");
            return View("Index");
        }



        public async Task<IActionResult> EditUser(int id)
        {
            //if (HttpContext.Session.GetString("role") == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}

            HttpResponseMessage reponseRole = await _client.GetAsync(RoleAPIUrl);
            string strDataRole = await reponseRole.Content.ReadAsStringAsync();
            
            HttpResponseMessage response = await _client.GetAsync(UserAPIUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            User? user = JsonSerializer.Deserialize<User?>(strData, options);
            List<Role>? listRole = JsonSerializer.Deserialize<List<Role>>(strDataRole, options);
            ViewBag.RoleId = new SelectList(listRole, "RoleId", "RoleName");

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            
            HttpResponseMessage responseRole = await _client.GetAsync(RoleAPIUrl);
            string strDataRole = await responseRole.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Role>? listRole = JsonSerializer.Deserialize<List<Role>>(strDataRole, options);
            ViewBag.RoleId = new SelectList(listRole, "RoleId", "RoleName");


            string jsonStr = JsonSerializer.Serialize(user);
            var contentData = new StringContent(jsonStr, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(UserAPIUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccMessage"] = "Edit Successfull!";
                return RedirectToAction("EditUser", "User");
            }
            ModelState.AddModelError(String.Empty, "Failed to call API!");
            return RedirectToAction("EditUser", "User");

        }

        public async Task<IActionResult> DetailUser(int id)
        {
            //if (HttpContext.Session.GetString("role") == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            HttpResponseMessage response = await _client.GetAsync(UserAPIUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            User lUser = JsonSerializer.Deserialize<User>(strData, options);
            return View(lUser);
        }

    }
}
