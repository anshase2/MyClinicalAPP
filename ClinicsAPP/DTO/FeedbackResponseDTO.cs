namespace ClinicsAPP.DTO
{
    public class FeedbackResponseDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
       // public int AppointmentId { get; set; }
        public int Rating { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public string? Comment { get; set; }
    }
}
