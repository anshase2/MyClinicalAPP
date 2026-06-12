using ClinicsAPP.DTO;


namespace ClinicsAPP.Contracts
{
    public interface IAppointmentService
    {
        Task<AppointmentResponseDTO> CreateAppointmentAsync(CreateAppointmentRequestDTO request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AppointmentResponseDTO>> GetAppointmentsByPatientAsync(int patientId, CancellationToken cancellationToken = default);
        Task<bool> HasAppointmentAsync(int patientId, int doctorId, CancellationToken cancellationToken = default);
        Task<int> CleanupPastAppointmentsAsync(CancellationToken cancellationToken = default);
        Task<bool> RejectAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default);
    }
}
