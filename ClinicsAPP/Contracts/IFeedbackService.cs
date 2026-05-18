using ClinicsAPP.DTO;

namespace ClinicsAPP.Contracts
{
    public interface IFeedbackService
    {
        Task<FeedbackResponseDTO> AddFeedbackAsync(int patientId, Guid doctorId, Guid appointmentId, int rating, string? comment, CancellationToken cancellationToken = default);
    }
}
