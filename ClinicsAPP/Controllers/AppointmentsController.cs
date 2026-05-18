using ClinicsAPP.Contracts;
using ClinicsAPP.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ClinicsAPP.Controllers
{
    [ApiController]
    [Route("appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AppointmentResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AppointmentResponseDTO>> Create(
            [FromBody] CreateAppointmentRequestDTO request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var created = await _appointmentService.CreateAppointmentAsync(
                    request.PatientId,
                    request.DoctorId,
                    request.AppointmentDate,
                    cancellationToken);

                return Created($"/appointments/{created.Id}", created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
