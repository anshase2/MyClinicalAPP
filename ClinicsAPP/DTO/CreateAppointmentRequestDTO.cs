using System.ComponentModel.DataAnnotations;

namespace ClinicsAPP.DTO
{
    public class CreateAppointmentRequestDTO
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }
    }
}
