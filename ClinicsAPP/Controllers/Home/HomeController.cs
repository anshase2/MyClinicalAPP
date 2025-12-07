using Microsoft.AspNetCore.Mvc;

namespace ClinicsAPP.Controllers
{
    public class HomeController : Controller
    {
       [HttpGet("/Home/ContactUs")]
        public IActionResult ContactUs()
        {

            return View();
        }
    }
}
