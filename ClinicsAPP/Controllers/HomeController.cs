using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace ClinicsAPP.Controllers
{
   //Ex: [Authorize(Roles ="Doctor")]
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

            return View("~/Views/Account/Login1.cshtml");
        }
    }
}
