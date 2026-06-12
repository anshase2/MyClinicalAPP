namespace ClinicsAPP.DTO
{
    public class AppointmentResponseDTO
    {
        public int Id { get; set; }
        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }

        public string Status { get; set; } = "Accepted";
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
       // public DoctorResponseDTO Doctor { get; set; }=new DoctorResponseDTO();
        public DateTime AppointmentDate { get; set; }
        
    }
}
