namespace ClinicsAPP.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
       // public int AppointmentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ?FeedbackDate { get; set; } = DateTime.Now;

        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
     //   public Appointment Appointment { get; set; } = null!;
    }
}
