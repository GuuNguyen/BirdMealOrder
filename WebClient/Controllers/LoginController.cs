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
                var parseUserId = Int32.Parse(userId);

                if (roleString == "Admin")
                {
                    HttpContext.Session.SetInt32("userID", parseUserId);
                    return RedirectToAction("Product_Index", "Admin");
                }
                else
                {
                    HttpContext.Session.SetInt32("userID", parseUserId);
                    var cart = new List<CartItem>();
                    HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
                    return RedirectToAction("Index", "");
                }
            }
            ModelState.AddModelError(String.Empty, "Your login is fail!");
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
                return RedirectToAction("Login", "Login");
            }
            return View(user);
        }
    }
}