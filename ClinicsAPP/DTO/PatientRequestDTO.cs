using ClinicsAPP.Models;
using ClinicsAPP.Models.IdentityModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicsAPP.DTO
{
    public class PatientRequestDTO
    {
        
        [Required]
        public string? FullName { get; set; }
        [Required]
        public Guid UserId { get; set; }





    }
}
