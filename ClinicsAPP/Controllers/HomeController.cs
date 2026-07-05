using ClinicsAPP.Contracts;
using ClinicsAPP.Data;
using ClinicsAPP.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClinicsAPP.Controllers
{
   //Ex: [Authorize(Roles ="Doctor")]
    public class HomeController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly ClinicsAPP.Services.DoctorService _doctorService;
        private readonly ApplicationDbContext _context;

        public HomeController(IPatientService patientService,
            IAppointmentService appointmentService,
            ClinicsAPP.Services.DoctorService doctorService,
            ApplicationDbContext context)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _context = context;
        }

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

        [HttpGet("/Home/MyAppointments")]
        public async Task<IActionResult> MyAppointments()
        {
            // ensure user is authenticated
            if (!User.Identity?.IsAuthenticated ?? true)
                return Challenge();

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userGuid))
            {
                return RedirectToAction("Login1");
            }

            // If user is a patient, use PatientService to get appointments (includes doctor name)
            if (User.IsInRole("Patient"))
            {
                try
                {
                    var details = await _patientService.GetPatientDetailsByUserId(userGuid);
                    
                    var appointments = details?.Appointments ?? new List<AppointmentResponseDTO>();
                    return View(appointments);
                }
                catch
                {
                    // If something goes wrong, return empty list to the view
                    return View(new List<AppointmentResponseDTO>());
                }
            }

            // If user is a doctor, query appointments by doctor's id
            if (User.IsInRole("Doctor"))
            {
                var doctor = await _doctorService.GetDoctorByUserId(userGuid);
                if (doctor == null)
                    return View(new List<AppointmentResponseDTO>());

                var appointments = await _context.Appointments
                    .AsNoTracking()
                    .Where(a => a.DoctorId == doctor.DoctorId)
                    .Include(a => a.Doctor)
                    .OrderBy(a => a.AppointmentDate)
                    .Select(a => new AppointmentResponseDTO
                    {
                        Id = a.Id,
                        PatientId = a.PatientId,
                        DoctorId = a.DoctorId,
                        AppointmentDate = a.AppointmentDate,
                        DoctorName = a.Doctor.FullName,
                        PatientName = a.Patient.FullName,
                        Status= a.Status
                    })
                    .ToListAsync();


                return View(appointments);
            }

            // default: return empty list
            return View(new List<AppointmentResponseDTO>());
        }
    }
}
