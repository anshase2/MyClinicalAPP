namespace ClinicsAPP.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string? FullName { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
