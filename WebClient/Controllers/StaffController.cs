﻿using BusinessObject.Enums;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories.DTOs.OrderDTO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
    public class StaffController : Controller
    {
        private readonly HttpClient client;
        private string ProductApiUrl = "";
        private string UserApiUrl = "";
        private string MealApiUrl = "";
        private string BirdApiUrl = "";
        private string OrderApiUrl = "";

        public StaffController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7022/api/Product";
            UserApiUrl = "https://localhost:7022/api/User";
            MealApiUrl = "https://localhost:7022/api/Meal";
            BirdApiUrl = "https://localhost:7022/api/Bird";
            OrderApiUrl = "https://localhost:7022/api/Order";
        }
        public async Task<IActionResult> Product_Index()
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

        public async Task<IActionResult> User_Index()
        {
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<User> listUser = JsonSerializer.Deserialize<List<User>>(strData, options);
            return View(listUser);
        }
        public async Task<IActionResult> Meal_Index()
        {
            HttpResponseMessage response = await client.GetAsync(MealApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Meal> listMeal = JsonSerializer.Deserialize<List<Meal>>(strData, options);
            return View(listMeal);
        }
        public async Task<IActionResult> Bird_Index()
        {
            HttpResponseMessage response = await client.GetAsync(BirdApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Bird> listBird = JsonSerializer.Deserialize<List<Bird>>(strData, options);
            return View(listBird);
        }
        public async Task<IActionResult> Order_Index()
        {
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Order> listOrder = JsonSerializer.Deserialize<List<Order>>(strData, options);

            ViewBag.OrderStatus = GetOrderStatusSelectList();

            return View(listOrder);
        }

        private SelectList GetOrderStatusSelectList()
        {
            var orderStatuses = Enum.GetValues(typeof(OrderStatus));
            return new SelectList(orderStatuses);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeOrderStatus(ChangeOrderStatusDTO order)
        {
            string strData = JsonSerializer.Serialize(order);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(OrderApiUrl + "/ChangeOrderStatus", contentData);
            if (response.IsSuccessStatusCode)
            {
                TempData["msg"] = "Update successfully!";
            }
            else
            {
                TempData["msg"] = "Something Went Wrong!";
            }
            return RedirectToAction("Order_Index", "Staff");
        }
    }
}
