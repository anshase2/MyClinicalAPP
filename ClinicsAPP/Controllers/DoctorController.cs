using Microsoft.AspNetCore.Mvc;
using ClinicsAPP.Models;

namespace ClinicsAPP.Controllers
{
    public class DoctorController : Controller
    {
        [HttpGet("/Doctorlist")]
        public IActionResult Index()
        {
            var doctors = new List<Doctor> { new Doctor { FullName = "osama anshasi" ,Specalist = "Heart" },
            new Doctor { FullName = "osama anshasi" ,Specalist = "Heart",Description="aa"},
            new Doctor { FullName = "osama anshasi" ,Specalist = "Heart" },
            new Doctor { FullName = "osama anshasi" ,Specalist = "Heart" } };
            return View( doctors);
        }
    }
}
