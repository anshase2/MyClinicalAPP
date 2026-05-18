namespace ClinicsAPP.DTO
{
    public class FeedbackResponseDTO
    {
        public Guid Id { get; set; }
        public int PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid AppointmentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
