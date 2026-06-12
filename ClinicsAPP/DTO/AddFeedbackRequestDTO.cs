using System.ComponentModel.DataAnnotations;

namespace ClinicsAPP.DTO
{
    public class AddFeedbackRequestDTO
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        /* [Required]
         public int AppointmentId { get; set; }?*/  

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }
    }
}
