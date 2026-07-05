namespace ClinicsAPP.DTO
{
    public class AppointmentResponseDTO
    {
        public int? Id { get; set; }
        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorImage { get; set; }
        public string Status { get; set; } = "Pending";
        public string? StatusMessage { get; set; }
        public bool? Success { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
       // public DoctorResponseDTO Doctor { get; set; }=new DoctorResponseDTO();
        public DateTime AppointmentDate { get; set; }
        
    }
}
