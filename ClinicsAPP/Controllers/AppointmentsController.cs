using ClinicsAPP.Contracts;
using ClinicsAPP.DTO;
using ClinicsAPP.Models;
using ClinicsAPP.Models.IdentityModels;
using ClinicsAPP.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;  
using System.Reflection.Metadata;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicsAPP.Controllers
{
    [Route("Appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly DoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentsController(IAppointmentService appointmentService, DoctorService doctorService, IPatientService patientService, UserManager<ApplicationUser> userManager)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _patientService = patientService;
            _userManager = userManager;
        }
        //   [HttpGet("/Appointments/book-appointment/{id}")]
        [HttpGet("book-appointment/{id}")]

        //[HttpGet]

        public async Task<IActionResult> DialogBooking(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
                return NotFound();

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Challenge();

            if (!Guid.TryParse(userIdClaim, out var userGuid))
                return BadRequest();

            PatientResponseDTO? patient = null;
            try
            {
                patient = await _patientService.GetPatientByUserId(userGuid);
            }
            catch
            {
                // ignore if not found
            }

            var appUser = await _userManager.FindByIdAsync(userIdClaim);

            var model = new CreateAppointmentRequestDTO
            {
                DoctorId = doctor.DoctorId,
                PatientId = patient?.PatientId ?? 0,
                AppointmentDate = default
            };

            ViewBag.FullName = patient?.FullName ?? (appUser != null ? $"{appUser.FirstName} {appUser.LastName}" : string.Empty);
            ViewBag.Email = appUser?.Email ?? string.Empty;
            ViewBag.Phone = appUser?.PhoneNumber ?? string.Empty;
            ViewBag.price = doctor.Price;
            ViewBag.DoctorName = doctor.FullName;
            ViewBag.Specalist = doctor.Specalist;


            return View("~/Views/Doctor/DialogBooking.cshtml", model);
        }
        [HttpPost("book-appointment/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DialogBookingPost(CreateAppointmentRequestDTO request, int id)
        {
            request.DoctorId = id;

            // تأكيد أن المريض الحالي هو المستخدم المسجل دخول
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var userGuid))
            {
                return BadRequest();
            }


            var patient = await _patientService.GetPatientByUserId(userGuid);

            if (patient == null)
                return Challenge();

            // ربط البيانات بشكل آمن
            request.PatientId = patient.PatientId;
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var appUser = await _userManager.FindByIdAsync(userIdClaim);

            // تحقق بسيط
            if (!ModelState.IsValid)
            {
                ViewBag.FullName = patient?.FullName ?? (appUser != null ? $"{appUser.FirstName} {appUser.LastName}" : string.Empty);
                ViewBag.Email = appUser?.Email ?? string.Empty;
                ViewBag.Phone = appUser?.PhoneNumber ?? string.Empty;
              //  ViewBag.price = doctor.Price;
               // ViewBag.DoctorName = doctor.FullName;
               // ViewBag.Specalist = doctor.Specalist;
                return View("~/Views/Doctor/DialogBooking.cshtml", request);
            }

            // حفظ الموعد
           var app= await _appointmentService.CreateAppointmentAsync(request);
            if (app.Success==false)
            {
                ViewBag.FullName = patient?.FullName ?? (appUser != null ? $"{appUser.FirstName} {appUser.LastName}" : string.Empty);
                ViewBag.Email = appUser?.Email ?? string.Empty;
                ViewBag.Phone = appUser?.PhoneNumber ?? string.Empty;
                var doctor = await _doctorService.GetDoctorByIdAsync(id);
                
                ViewBag.price = doctor.Price;
                ViewBag.DoctorName = doctor.FullName;

                ViewBag.Specalist = doctor.Specalist;

                TempData["ErrorMessage"] = app.StatusMessage;
                request.AppointmentDate = default; // إعادة تعيين التاريخ لعرض رسالة الخطأ فقط
               
                return View("~/Views/Doctor/DialogBooking.cshtml", app);

            }
          
            TempData["SuccessMessage"] = "Appointment created successfully.";
            // نجاح → تحويل لصفحة البروفايل
            return RedirectToAction("MyProfile", "Patient");
        }

        [HttpGet("reject-appointment/{id}")]
        public async Task<IActionResult> RejectAppointment(int id)
        {
            if (!await _appointmentService.RejectAppointmentAsync(id))
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Appointment rejected successfully.";

            return RedirectToAction("MyAppointments", "Home");
        }
        [HttpGet("accept-appointment/{id}")]
        public async Task<IActionResult> AcceptAppointment(int id)
        {
            if (!await _appointmentService.AcceptAppointmentAsync(id))
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Appointment accepted successfully.";

            return RedirectToAction("MyAppointments", "Home");
        }
        [HttpGet("cancel-appointment/{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            if (!await _appointmentService.CancelAppointmentAsync(id))
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Appointment cancelled successfully.";

            return RedirectToAction("MyAppointments", "Home");
        }

        // [HttpGet("/Doctor/book-appointment/{id}")]
        /*  [HttpPost]
           public async Task<IActionResult> DialogBookingPost(int id)
           {
               var doctor = await _doctorService.GetDoctorByIdAsync(id);
               if (doctor == null)
                   return NotFound();

               var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
               if (string.IsNullOrEmpty(userIdClaim))
                   return Challenge();

               if (!Guid.TryParse(userIdClaim, out var userGuid))
                   return BadRequest();

               PatientResponseDTO? patient = null;
               try
               {
                   patient = await _patientService.GetPatientByUserId(userGuid);
               }
               catch
               {
                   // ignore if not found
               }

               var appUser = await _userManager.FindByIdAsync(userIdClaim);

               var model = new CreateAppointmentRequestDTO
               {
                   DoctorId = doctor.DoctorId,
                   PatientId = patient?.PatientId ?? 0,
                   AppointmentDate = DateTime.Now
               };

               ViewBag.FullName = patient?.FullName ?? (appUser != null ? $"{appUser.FirstName} {appUser.LastName}" : string.Empty);
               ViewBag.Email = appUser?.Email ?? string.Empty;
               ViewBag.Phone = appUser?.PhoneNumber ?? string.Empty;
               ViewBag.price = doctor.Price;
               ViewBag.DoctorName = doctor.FullName;
               ViewBag.Specalist = doctor.Specalist;

               return View("~/Views/Doctor/DialogBooking.cshtml", model);
           }

           [HttpPost("/Appointments/book-appointment")]
           public async Task<IActionResult> DialogBookingPost(
         int id,
         CreateAppointmentRequestDTO request
         )
           {
               // تأكيد الدكتور من الـ route فقط (مصدر موثوق)
               request.DoctorId = id;

               // Validation
               if (!ModelState.IsValid)
               {
                   var doctor = await _doctorService.GetDoctorByIdAsync(id);

                   var appUser = await _userManager.FindByIdAsync(
                       User.FindFirstValue(ClaimTypes.NameIdentifier));

                   ViewBag.FullName = appUser != null ? $"{appUser.FirstName} {appUser.LastName}" : string.Empty;
                   ViewBag.Email = appUser?.Email ?? string.Empty;
                   ViewBag.Phone = appUser?.PhoneNumber ?? string.Empty;
                   ViewBag.DoctorName = doctor.FullName;
                   ViewBag.Specialist = doctor.Specalist;

                   ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                   return View("~/Views/Doctor/DialogBooking.cshtml",
                       new CreateAppointmentRequestDTO {

                           DoctorId = id,
                           PatientId = request.PatientId,
                         AppointmentDate = request.AppointmentDate   

                       });       
               }

               try
               {
                   await _appointmentService.CreateAppointmentAsync(request);

                   TempData["Message"] = "Appointment created successfully.";
                   //send nootification 
                   return RedirectToAction("MyProfile", "Patient");
               }
               catch (InvalidOperationException ex)
               {
                   ModelState.AddModelError(string.Empty, ex.Message);

                   var doctor = await _doctorService.GetDoctorByIdAsync(id);

                   return View("~/Views/Doctor/DialogBooking.cshtml",
                     new CreateAppointmentRequestDTO
                     {

                         DoctorId = id,
                         PatientId = request.PatientId,
                         AppointmentDate = request.AppointmentDate

                     });
               }
           }*/


        [HttpPost]
            [ProducesResponseType(typeof(AppointmentResponseDTO), StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<AppointmentResponseDTO>> Create(
            [FromBody] CreateAppointmentRequestDTO request,
            CancellationToken cancellationToken)
            {

                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                try {
                    var created = await _appointmentService.CreateAppointmentAsync(request, cancellationToken);

                return Created($"/appointments/{created.Id}", created);
              
            }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

        } }
