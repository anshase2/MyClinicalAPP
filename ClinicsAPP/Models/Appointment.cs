using System.ComponentModel.DataAnnotations;

namespace ClinicsAPP.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "Pending";
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
