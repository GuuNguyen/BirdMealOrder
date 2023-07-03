using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.FeedbackDTO;
using System.Net.Http.Headers;
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
            return RedirectToAction("FeedbackSuccess", feebackSuccessViewModel);
        }
        public async Task<IActionResult> FeedbackSuccess(FeebackSuccessViewModel feebackSuccessViewModel)
        {
            return View(feebackSuccessViewModel);
        }
    }
}
