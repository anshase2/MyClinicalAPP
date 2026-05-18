using ClinicsAPP.DTO;

namespace ClinicsAPP.Contracts
{
    public interface IAppointmentService
    {
        Task<AppointmentResponseDTO> CreateAppointmentAsync(int patientId, Guid doctorId, DateTime appointmentDate, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AppointmentResponseDTO>> GetAppointmentsByPatientAsync(int patientId, CancellationToken cancellationToken = default);
        Task<bool> HasAppointmentAsync(int patientId, Guid doctorId, CancellationToken cancellationToken = default);
    }
}
