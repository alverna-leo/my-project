using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StyleBay_Version2.Models;

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
