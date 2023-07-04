using Azure;
using BusinessObject;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.OrderDTO;
using Repositories.DTOs.ShippingAddressDTO;
using System.Net.Http.Headers;
using System.Text.Json;
using WebClient.ViewModels;
using WebClient.VIewModels;

namespace WebClient.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly HttpClient _client;
        private string MealAPIUrl = "";
        private string ProductAPIUrl = "";
        private string OrderAPIUrl = "";
        private string SAAPIUrl = "";

        public CheckoutController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            MealAPIUrl = "https://localhost:7022/api/Meal";
            ProductAPIUrl = "https://localhost:7022/api/Product";
            OrderAPIUrl = "https://localhost:7022/api/Order";
            SAAPIUrl = "https://localhost:7022/api/ShippingAddress";
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var strCart = HttpContext.Session.GetString("cart");
            var userId = HttpContext.Session.GetInt32("userID");
            bool haveMeal = false;
            bool haveProduct = false;
            List<Meal>? listMeal = null;
            List<Product>? listProduct = null;
            List<int> listMealId = new List<int>();
            List<int> listProductId = new List<int>();
            if (userId == null) return Redirect("/Login/Login");
            if (string.IsNullOrEmpty(strCart))
            {
                GetCartViewModel data = new GetCartViewModel();
                return View(data);
            }
            var list = JsonSerializer.Deserialize<List<CartItem>>(strCart);
            foreach (var item in list)
            {
                if (item.ProductId.HasValue)
                {
                    listProductId.Add(item.ProductId.Value);
                    haveProduct = true;
                }
                if (item.MealId.HasValue)
                {
                    listMealId.Add(item.MealId.Value);
                    haveMeal = true;
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (haveMeal)
            {
                var listMealIdSerialize = JsonSerializer.Serialize(listMealId);
                var contentData = new StringContent(listMealIdSerialize, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage mealResponse = await _client.PostAsync(MealAPIUrl + "/Meals", contentData);
                string mealStrData = await mealResponse.Content.ReadAsStringAsync();
                listMeal = JsonSerializer.Deserialize<List<Meal>>(mealStrData, options);
            }
            if (haveProduct)
            {
                var listProductIdSerialize = JsonSerializer.Serialize(listProductId);
                var contentData = new StringContent(listProductIdSerialize, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage productResponse = await _client.PostAsync(ProductAPIUrl + "/Products", contentData);
                string productStrData = await productResponse.Content.ReadAsStringAsync();
                listProduct = JsonSerializer.Deserialize<List<Product>>(productStrData, options);
            }
            HttpResponseMessage SAResponse = await _client.GetAsync(SAAPIUrl + $"/ListBy/{userId}");
            string SAStrData = await SAResponse.Content.ReadAsStringAsync();
            var listSA = JsonSerializer.Deserialize<List<GetSADTO>>(SAStrData, options);
            var finalResult = new GetCartViewModel
            {
                Meals = listMeal ?? new List<Meal>(),
                Products = listProduct ?? new List<Product>(),
                CartItems = list,
                ShippingAddresses = listSA,
            };
            return View(finalResult);
        }


        public async Task<IActionResult> AddToCart(string code, int quantity)
        {
            var strCart = HttpContext.Session.GetString("cart");
            var userId = HttpContext.Session.GetInt32("userID");
            if (userId == null) return Json(new { redirectToLogin = true });
            var list = JsonSerializer.Deserialize<List<CartItem>>(strCart);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Meal? meal = null;
            Product? product = null;
            string prefix = code.Substring(0, 2);
            switch (prefix)
            {
                case "ME":
                    HttpResponseMessage mealResponse = await _client.GetAsync(MealAPIUrl + $"/Detail/{code}");
                    string mealStrData = await mealResponse.Content.ReadAsStringAsync();
                    meal = JsonSerializer.Deserialize<Meal>(mealStrData, options);
                    break;
                case "FO":
                    HttpResponseMessage productResponse = await _client.GetAsync(ProductAPIUrl + $"/Detail/{code}");
                    string productStrData = await productResponse.Content.ReadAsStringAsync();
                    product = JsonSerializer.Deserialize<Product>(productStrData, options);
                    break;
            }
            if (list == null)
            {
                list = new List<CartItem>();
                CartItem cart = new CartItem
                {
                    MealId = meal?.MealId,
                    ProductId = product?.ProductId,
                    Quantity = quantity
                };
                list.Add(cart);
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(list));
                return Ok();
            }
            else if (!list.Any(f => f.MealId == meal?.MealId) || !list.Any(p => p.ProductId == product?.ProductId))
            {
                CartItem cart = new CartItem
                {
                    MealId = meal?.MealId,
                    ProductId = product?.ProductId,
                    Quantity = quantity
                };
                list.Add(cart);
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(list));
                return Ok();
            }
            if (product != null)
            {
                var item = list.SingleOrDefault(f => f.ProductId == product.ProductId);
                if (item != null)
                {
                    item.Quantity += quantity;
                }
            }
            else
            {
                var item = list.SingleOrDefault(m => m.MealId == meal?.MealId);
                if (item != null)
                {
                    item.Quantity += quantity;
                }

            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(list));
            return Ok();
        }

        public async Task<IActionResult> Order(int shippingAddressId)
        {
            var strCart = HttpContext.Session.GetString("cart");
            var userId = HttpContext.Session.GetInt32("userID");
            string apiUrl;
            int id;
            if (strCart == null || string.IsNullOrEmpty(strCart)) return Json(new { cartIsEmpty = true });
            var list = JsonSerializer.Deserialize<List<CartItem>>(strCart);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            if (list?.Count > 0)
            {
                foreach (var item in list)
                {
                    var isProduct = false;
                    if (item.Quantity <= 0)
                    {
                        return BadRequest(Json(new JsonMessageViewModel
                        {
                            ErrorMessage = $"The quantity must be more than 0"
                        }));
                    }
                    if (item.ProductId.HasValue)
                    {
                        apiUrl = ProductAPIUrl;
                        id = item.ProductId.Value;
                        isProduct = true;
                    }
                    else
                    {
                        apiUrl = MealAPIUrl;
                        id = item.MealId.Value;
                    }
                    HttpResponseMessage response = await _client.GetAsync(apiUrl + $"/{id}");
                    string strData = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(strData))
                    {

                    }
                    if (isProduct)
                    {
                        Product? responseObj = JsonSerializer.Deserialize<Product>(strData, options);
                        if (item.Quantity > responseObj?.QuantityAvailable)
                        {
                            return BadRequest(Json(new JsonMessageViewModel
                            {
                                ErrorMessage = $"The quantity of {responseObj.ProductName} is only {responseObj.QuantityAvailable} "
                            }));
                        }
                    }
                    else
                    {
                        Meal? responseObj = JsonSerializer.Deserialize<Meal>(strData, options);
                        if (item.Quantity > responseObj?.QuantityAvailable)
                        {
                            return BadRequest(Json(new JsonMessageViewModel
                            {
                                ErrorMessage = $"The quantity of {responseObj.MealName} is only {responseObj.QuantityAvailable} "
                            }));
                        }
                    }
                }
                var newOrder = new CreateOrderDTO
                {
                    UserId = userId.Value,
                    ShippingAddressId = shippingAddressId,
                    CartItems = list
                };
                var listCartSerialize = JsonSerializer.Serialize(newOrder, options);
                var contentData = new StringContent(listCartSerialize, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage listCartResponse = await _client.PostAsync(OrderAPIUrl, contentData);
                if (listCartResponse.IsSuccessStatusCode)
                {
                    HttpContext.Session.Remove("cart");
                    var cart = new List<CartItem>();
                    HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
                    return Ok(Json(new JsonMessageViewModel
                    {
                        SuccessMessage = "Your order is successfully created"
                    }));
                }
            }
            return BadRequest(Json(new JsonMessageViewModel
            {
                ErrorMessage = "Cart is Empty"
            }));
        }
        public  IActionResult UpdateQuantity(int id, int quantity)
        {
            var strCart = HttpContext.Session.GetString("cart");
            var list = JsonSerializer.Deserialize<List<CartItem>>(strCart);

            var item = list.SingleOrDefault(f => f.ProductId == id || f.MealId == id);
            if (item != null)
            {
                if(quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    list.Remove(item);
                }           
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(list));
                return Ok();
            }
            return BadRequest();
        }
    }
}
