using System.ComponentModel.DataAnnotations;

namespace ClinicsAPP.DTO
{
    public class CreateAppointmentRequestDTO
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }
    }
}
