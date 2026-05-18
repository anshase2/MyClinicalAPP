namespace ClinicsAPP.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public int PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }

        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
