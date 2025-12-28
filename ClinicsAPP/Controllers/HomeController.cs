using Microsoft.AspNetCore.Mvc;

namespace ClinicsAPP.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Home()
        {

            return View();
        }
        [HttpGet("/Home/ContactUs")]
        public IActionResult ContactUs()
        {

            return View();
        }
        [HttpGet("/Home/Login")]
        public IActionResult Login1()
        {
            ViewBag.Title = "Login";

            return View();
        }
    }
}
