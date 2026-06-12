using ClinicsAPP.Models;
using ClinicsAPP.Models.IdentityModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicsAPP.DTO
{
    public class PatientResponseDTO
    {

     
        public int PatientId { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;

      //  public ICollection<AppointmentResponseDTO> Appointments { get; set; } = new List<AppointmentResponseDTO>();
        //public ICollection<FeedbackResponseDTO> Feedbacks { get; set; } = new List<FeedbackResponseDTO>();
    }
}
