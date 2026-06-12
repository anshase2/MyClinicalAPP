namespace ClinicsAPP.DTO
{
    public class PatientDetailsResponseDTO
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = null!;
        public Guid UserId { get; set; }
        public DateTime? DateOfBirth { get; set; }
                public string? Gender { get; set; }
       
        public List<AppointmentResponseDTO> Appointments { get; set; } = new();
        public List<FeedbackResponseDTO> Feedbacks { get; set; } = new();
    }
}
