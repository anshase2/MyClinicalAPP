using System.ComponentModel.DataAnnotations;

namespace ClinicsAPP.DTO
{
    public class AddFeedbackRequestDTO
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public Guid AppointmentId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }
    }
}
