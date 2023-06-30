using Microsoft.AspNetCore.Mvc;

namespace WebClient.ViewModels
{
    public class CheckoutController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }
    }
}
