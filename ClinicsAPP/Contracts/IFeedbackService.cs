using ClinicsAPP.DTO;

namespace ClinicsAPP.Contracts
{
    public interface IFeedbackService
    {
        Task<FeedbackResponseDTO> AddFeedbackAsync(AddFeedbackRequestDTO request, CancellationToken cancellationToken = default);
        Task<IEnumerable<FeedbackResponseDTO>> GetDoctorFeedbacksAsync(int doctorId, CancellationToken cancellationToken = default);
        Task<double> GetDoctorAverageRatingAsync(int doctorId, CancellationToken cancellationToken = default);
    }
}
