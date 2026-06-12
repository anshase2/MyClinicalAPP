using AspNetCoreGeneratedDocument;
using ClinicsAPP.Contracts;
using ClinicsAPP.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicsAPP.Controllers
{
    [Route("[controller]/[action]")]

    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: /Patient
        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetAllPatients();
            return View(patients);
        }

        public async Task<IActionResult> MyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var patient = await _patientService.GetPatientDetailsByUserId(Guid.Parse(userId));

            if (patient == null)
                return NotFound();

            var dto = new PatientDetailsResponseDTO
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName!,
                UserId = patient.UserId,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
            


                Appointments = patient.Appointments.Select(a => new AppointmentResponseDTO
                {
                    Id = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    DoctorName = a.DoctorName 


                }).ToList(),

                Feedbacks = patient.Feedbacks.Select(f => new FeedbackResponseDTO
                {
                    Id = f.Id,
                    Comment = f.Comment
                }).ToList()
            };

            return View("PatientProfile", dto);
        }

        // GET: /Patient/Details/5
        public async Task<IActionResult> Profile(int id)
        {
            if (id <= 0)
                return BadRequest();

            var patient = await _patientService.GetPatientDetails(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // GET: /Patient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _patientService.CreatePatient(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: /Patient/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var patient = await _patientService.GetPatientById(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // POST: /Patient/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _patientService.DeletePatient(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}
