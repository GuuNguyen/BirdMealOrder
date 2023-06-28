﻿using BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Repositories.DTOs.AccountDTO;
using Repositories.DTOs.UserDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace WebClient.Controllers
{
    public class LoginController : Controller
    {

        private readonly HttpClient _client;
        private string LoginAPIUrl = "";
        private string UserAPIUrl = "";

        public LoginController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            LoginAPIUrl = "https://localhost:7022/api/Login";
            UserAPIUrl = "https://localhost:7022/api/User";
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountDTO account)
        {
            string strData = JsonSerializer.Serialize(account);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(LoginAPIUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                string result = await response.Content.ReadAsStringAsync();
                var temp = JObject.Parse(result);
                var token = temp["token"].ToString();
                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                var roleString = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
                var userId = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;
                var userFullName = jwtSecurityToken.Claims.First(claim => claim.Type == "FullName").Value;
                string splipedName = "Name";
                int lastSpaceIndex = userFullName.LastIndexOf(' ');                
                if (lastSpaceIndex != -1 && lastSpaceIndex < userFullName.Length - 1)
                {
                    splipedName = userFullName.Substring(lastSpaceIndex + 1);
                }
                HttpContext.Session.SetString("userName", splipedName);
                var parseUserId = Int32.Parse(userId);

                if (roleString == "Admin")
                {
                    HttpContext.Session.SetInt32("userID", parseUserId);
                    return RedirectToAction("User_Index", "Staff");
                }
                else if(roleString == "Staff")
                {
                    HttpContext.Session.SetInt32("userID", parseUserId);
                    return RedirectToAction("User_Index", "Staff");
                }
                else
                {
                    HttpContext.Session.SetInt32("userID", parseUserId);
                    var cart = new List<CartItem>();
                    HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.ErrorMessage = "Your login has failed!"; 
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateUserDTO user)
        {
            string strData = JsonSerializer.Serialize(user);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(UserAPIUrl, contentData);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("/Login/Login");
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
