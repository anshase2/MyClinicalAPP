using ClinicsAPP.Contracts;
using ClinicsAPP.DTO;
using ClinicsAPP.Models.IdentityModels;
using ClinicsAPP.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures; // Add this using directive

namespace ClinicsAPP.Controllers
{

    [Route("feedback")]
    public class FeedbackController : Controller // Change from ControllerBase to Controller
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IPatientService _patientService;

        private readonly UserManager<ApplicationUser> _userManager;


        public FeedbackController(IFeedbackService feedbackService, IPatientService patientService, UserManager<ApplicationUser> userManager)
        {
            _patientService = patientService;
            _feedbackService = feedbackService;
            _userManager = userManager;
        }

        [HttpPost]
        [ProducesResponseType(typeof(FeedbackResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FeedbackResponseDTO>> Add(
            [FromBody] AddFeedbackRequestDTO request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var created = await _feedbackService.AddFeedbackAsync(
                    request,
                    cancellationToken);

                return Created($"/feedback/{created.Id}", created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Accept feedback submissions from Razor Pages / MVC forms
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] AddFeedbackRequestDTO request, CancellationToken cancellationToken)
        {
            // ????? ?? ?????? ?????? ?? ???????? ?????? ????
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var userGuid))
            {
                return BadRequest();
            }


            var patient = await _patientService.GetPatientByUserId(userGuid); 
            request.PatientId = patient.PatientId; // ????? ?? ?????? ?????? ?? ???????? ?????? ????

            if (!ModelState.IsValid)
            {

                // Redirect back to the doctor's profile page if form validation fails
                TempData["ErrorMessage"] = "Please fill all required fields correctly.";

                return RedirectToAction("DoctorProfile", "Doctor", new { id = request.DoctorId });
            }

            try
            {
                var created = await _feedbackService.AddFeedbackAsync(request, cancellationToken);
                TempData["SuccessMessage"] = "Feedback submitted successfully.";

                // After creating feedback, redirect back to the doctor's profile
                return RedirectToAction("DoctorProfile", "Doctor", new { id = request.DoctorId });
            }
            catch (InvalidOperationException ex)
            {
                // Store error message and redirect back so the UI can show it (if desired)
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("DoctorProfile", "Doctor", new { id = request.DoctorId });
            }
        }
    }
}
