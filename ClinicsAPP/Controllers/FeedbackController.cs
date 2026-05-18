using ClinicsAPP.Contracts;
using ClinicsAPP.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClinicsAPP.Controllers
{
    [ApiController]
    [Route("feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
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
                    request.PatientId,
                    request.DoctorId,
                    request.AppointmentId,
                    request.Rating,
                    request.Comment,
                    cancellationToken);

                return Created($"/feedback/{created.Id}", created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
