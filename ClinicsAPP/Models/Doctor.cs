using System.ComponentModel.DataAnnotations;
using ClinicsAPP.Models.IdentityModels;

namespace ClinicsAPP.Models
{
    public class Doctor
    {
        [Key]
        public Guid DoctorId { get; set; }
        public string? FullName { set; get; }
        public string? Specalist { set; get; }
        public string? Description { set; get; }

      
    
        public double? Rating { get; set; }
        public int ReviewCount { get; set; }
        public string? Location { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid UserId { get; set; }

        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
       // public bool IsApproved { get; set; }
        public bool? IsAvailable { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    }
}