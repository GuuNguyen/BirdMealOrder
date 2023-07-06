using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.FeedbackDTO;
using System.Net.Http.Headers;
using System.Text.Json;
using WebClient.ViewModels;

namespace WebClient.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly HttpClient client;
        private string FeedbackAPI = "";

        public FeedbackController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FeedbackAPI = "https://localhost:7022/api/Feedback";
        }

        [HttpPost]
        public async Task<IActionResult> Feedback(FeebackSuccessViewModel feebackSuccessViewModel)
        {
            if (HttpContext.Session.GetString("role") != "Customer")
            {
                return Redirect("/Login/Login");
            }
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var feedback = new CreateFeedbackDTO
            {
                OrderDetailId = feebackSuccessViewModel.OrderDetailId,
                Rating = feebackSuccessViewModel.Rating,
                Feedback = feebackSuccessViewModel.Feedback
            };
            string strData = JsonSerializer.Serialize(feedback);
            var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(FeedbackAPI, contentData);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("FeedbackSuccess", feebackSuccessViewModel);
            }
            else
            {
                TempData["msg"] = "Something Went Wrong!";
                return RedirectToAction("OrderDetail", "Order", feebackSuccessViewModel.OrderDetailId);
            }

        }
        public async Task<IActionResult> FeedbackSuccess(FeebackSuccessViewModel feebackSuccessViewModel)
        {
            if (HttpContext.Session.GetString("role") != "Customer")
            {
                return Redirect("/Login/Login");
            }
            return View(feebackSuccessViewModel);
        }
    }
}
