using ClinicsAPP.Contracts;
using ClinicsAPP.Data;
using ClinicsAPP.DTO;
using ClinicsAPP.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicsAPP.Services
{
    public class FeedbackService :IFeedbackService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public FeedbackService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

       public async Task<FeedbackResponseDTO> AddFeedbackAsync(AddFeedbackRequestDTO request, CancellationToken cancellationToken = default)
        {
           /* var appointment = await _context.Appointments
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);*/
            var appointmentCount = await _context.Appointments
    .CountAsync(a => a.PatientId == request.PatientId && a.DoctorId == request.DoctorId && a.Status == "Accepted" &&
        a.AppointmentDate < DateTime.UtcNow);

            var feedbackCount = await _context.Feedbacks
                .CountAsync(f => f.PatientId == request.PatientId && f.DoctorId == request.DoctorId);

            if (feedbackCount >= appointmentCount)
                throw new InvalidOperationException("No available appointments to review");
           /* if (appointment == null
                || appointment.PatientId != request.PatientId
                || appointment.DoctorId != request.DoctorId)
            {
                throw new InvalidOperationException("Patient did not attend this doctor");
            }*/

            var feedback = new Feedback
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
               // AppointmentId = request.AppointmentId,
                Rating = request.Rating,
                Comment = request.Comment,
                FeedbackDate = DateTime.UtcNow,
                Patient = await _context.Patients.FindAsync(request.PatientId),
                Doctor=await _context.Doctors.FindAsync(request.DoctorId),
                



            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync(cancellationToken);
            // Send notification to doctor (optional)
            await _notificationService.SendAsync(feedback.Doctor.UserId, "New Feedback", $"You have received new feedback from {feedback.Patient.FullName}", "dsad");

            return new FeedbackResponseDTO
            {
                Id = feedback.Id,
                PatientId = feedback.PatientId,
                DoctorId = feedback.DoctorId,
               // AppointmentId = feedback.AppointmentId,
                Rating = feedback.Rating,
                FeedbackDate = feedback.FeedbackDate,
                Comment = feedback.Comment,
                PatientName = _context.Patients
            .Where(p => p.PatientId == feedback.PatientId)
            .Select(p => p.FullName)
            .FirstOrDefault() ?? string.Empty,

            };
        }

        public async Task<IEnumerable<FeedbackResponseDTO>> GetDoctorFeedbacksAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            var feedbacks = await _context.Feedbacks
                .AsNoTracking()
                .Where(f => f.DoctorId == doctorId)
                .Select(f => new FeedbackResponseDTO
                {
                    Id = f.Id,
                    PatientId = f.PatientId,
                    DoctorId = f.DoctorId,
                 //   AppointmentId = f.AppointmentId,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    FeedbackDate = f.FeedbackDate,
                    PatientName = _context.Patients
            .Where(p => p.PatientId == f.PatientId)
            .Select(p => p.FullName)
            .FirstOrDefault() ?? string.Empty
                })
                .ToListAsync(cancellationToken);

            return feedbacks;
        }

        public async Task<double> GetDoctorAverageRatingAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            var average = await _context.Feedbacks
                .AsNoTracking()
                .Where(f => f.DoctorId == doctorId)
                .Select(f => (double?)f.Rating)
                .AverageAsync(cancellationToken);

            return average ?? 0.0;
        }
    }
}
