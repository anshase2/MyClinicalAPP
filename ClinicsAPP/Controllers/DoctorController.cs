using ClinicsAPP.Contracts;
using ClinicsAPP.DTO;
using ClinicsAPP.Models;
using ClinicsAPP.Models.IdentityModels;
using ClinicsAPP.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using ClinicsAPP.Data;

namespace ClinicsAPP.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFeedbackService _feedbackService;
        private readonly ApplicationDbContext _context;

        public DoctorController(DoctorService doctorService, IPatientService patientService, UserManager<ApplicationUser> userManager, IFeedbackService feedbackService, ApplicationDbContext context)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _userManager = userManager;
            _feedbackService = feedbackService;
            _context = context;
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
            doctor.AppointmentsCount = await _doctorService.GetDoctorAppointmentsCountAsync(id);
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
            doctor.AppointmentsCount = await _doctorService.GetDoctorAppointmentsCountAsync(doctor.DoctorId);


            return View("~/Views/Doctor/DoctorProfile.cshtml", doctor);
        }
        [HttpGet("/Doctor/Search")]
        public async Task<IActionResult> Search(DoctorSearchDTO model)
        {
            var query = _context.Doctors.AsQueryable();

            if (!string.IsNullOrEmpty(model.DoctorName))
            {
                query = query.Where(d => d.FullName.Contains(model.DoctorName));
                ViewBag.DoctorName = model.DoctorName;
            }

            if (!string.IsNullOrEmpty(model.Gender))
            {
                query = query.Where(d => d.User.Gender == model.Gender);
                ViewBag.Gender = model.Gender;
            }

            if (!string.IsNullOrEmpty(model.Location))
            {
                query = query.Where(d => d.Location == model.Location);
                ViewBag.Location = model.Location;
            }

            if (!string.IsNullOrEmpty(model.Specialty))
            {
                query = query.Where(d => d.Specalist == model.Specialty);
                ViewBag.Specialty = model.Specialty;
            }
            if (!string.IsNullOrEmpty(model.SortBy))
            {
                switch (model.SortBy)
                {
                    case "Rating":
                        query = query.OrderByDescending(d => d.Feedbacks.Average(f => (double?)f.Rating) ?? 0);
                        break;
                    case "Price":
                        query = query.OrderBy(d => d.Price);
                        break;
                    default:
                        break;
                }
                ViewBag.SortBy = model.SortBy;

            }

            var doctors = await query
     .Include(d => d.User)
     .ToListAsync();

            var result = new List<DoctorResponseDTO>();

            foreach (var doctor in doctors)
            {
                var avgRating = await _context.Feedbacks
                    .Where(f => f.DoctorId == doctor.DoctorId)
                    .AverageAsync(f => (double?)f.Rating) ?? 0;

                result.Add(doctor.ToDoctorResponseDTO(Math.Round(avgRating, 1)));
            }


            return View("DocList", result);
        }


    }
}
