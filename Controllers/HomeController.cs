using Microsoft.AspNetCore.Mvc;

namespace StyleBay_Version2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
