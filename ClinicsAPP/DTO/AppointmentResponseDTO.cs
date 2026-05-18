namespace ClinicsAPP.DTO
{
    public class AppointmentResponseDTO
    {
        public Guid Id { get; set; }
        public int PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
