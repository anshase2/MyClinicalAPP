using ClinicsAPP.Contracts;
using ClinicsAPP.Data;
using ClinicsAPP.DTO;
using ClinicsAPP.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicsAPP.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ApplicationDbContext _context;

        public FeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FeedbackResponseDTO> AddFeedbackAsync(int patientId, Guid doctorId, Guid appointmentId, int rating, string? comment, CancellationToken cancellationToken = default)
        {
            var appointment = await _context.Appointments
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

            if (appointment == null
                || appointment.PatientId != patientId
                || appointment.DoctorId != doctorId)
            {
                throw new InvalidOperationException("Patient did not attend this doctor");
            }

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentId = appointmentId,
                Rating = rating,
                Comment = comment
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync(cancellationToken);

            return new FeedbackResponseDTO
            {
                Id = feedback.Id,
                PatientId = feedback.PatientId,
                DoctorId = feedback.DoctorId,
                AppointmentId = feedback.AppointmentId,
                Rating = feedback.Rating,
                Comment = feedback.Comment
            };
        }
    }
}
