using Microsoft.AspNetCore.Mvc;
using ClinicsAPP.Models;

namespace ClinicsAPP.Controllers
{
    public class DoctorController : Controller
    {
        [HttpGet("/Doctorlist")]
        public IActionResult DocList()
        {
            var doctors = new List<Doctor> { new Doctor { FullName = "osama anshasi" ,Specalist = "Heart" },
            new Doctor { FullName = "osama anshasi" ,Specalist = "Heart",Description="aa"},
            new Doctor { FullName = "osama anshasi" ,Specalist = "Heart" },
            new Doctor { FullName = "osama anshasi" ,Specalist = "Heart" ,} };
            ViewBag.Title = "Doctor List";
            return View( doctors);
        }
        [HttpGet("/DoctorProfile")]
        public IActionResult DocProfile()
        {
          
            return View();
        }
        [HttpGet("/Doctor/book-appointment")]
        public IActionResult DialogBooking()
        {

            return View();
        }
    }
}
