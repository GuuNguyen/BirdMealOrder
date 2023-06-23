using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers
{
    public class MealController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
