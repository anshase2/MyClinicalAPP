using ClinicsAPP.DTO;

namespace ClinicsAPP.Contracts
{
    public interface IPatientService
    {
        Task<List<PatientResponseDTO>> GetAllPatients();
        Task<PatientResponseDTO?> GetPatientById(int id);

        Task<PatientDetailsResponseDTO?> GetPatientDetails(int id);
        Task<PatientResponseDTO?> GetPatientByUserId(Guid userId);
        Task<PatientDetailsResponseDTO?> GetPatientDetailsByUserId(Guid userId);
        Task<int> CreatePatient(PatientRequestDTO dto);

        Task<bool> DeletePatient(int id);
    }
}
