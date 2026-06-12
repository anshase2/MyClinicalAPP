using ClinicsAPP.Models;
using ClinicsAPP.Services;
using ClinicsAPP.DTO;
using ClinicsAPP.Contracts;
using ClinicsAPP.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using System.Threading.Tasks;

namespace ClinicsAPP.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFeedbackService _feedbackService;

        public DoctorController(DoctorService doctorService, IPatientService patientService, UserManager<ApplicationUser> userManager, IFeedbackService feedbackService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _userManager = userManager;
            _feedbackService = feedbackService;
        }

        [HttpGet("/Doctorlist")]
        public async Task<IActionResult> DocList()
        {
            var doctors =await  _doctorService.GetAllDoctorsAsync();
            ViewBag.Title = "Doctor List";
            return View( doctors);
        }
        [HttpGet("/DoctorProfile/{id}")]
        public async Task<IActionResult> DoctorProfile(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            doctor.AverageRating = await _feedbackService.GetDoctorAverageRatingAsync(id);
            if (doctor == null)
                return NotFound();
            ViewBag.Feedbacks = await _feedbackService.GetDoctorFeedbacksAsync(id);
            return View(doctor);
         

        }
        [HttpGet("/MyDoctorProfile")]
        public async Task<IActionResult> MyDoctorProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var doctor = await _doctorService.GetDoctorByUserId(Guid.Parse(userId));

            return View("~/Views/Doctor/DoctorProfile.cshtml", doctor);
        }



    }
}
