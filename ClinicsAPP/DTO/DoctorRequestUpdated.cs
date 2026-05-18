using System.ComponentModel.DataAnnotations;

namespace ClinicsAPP.DTO
{
    public class DoctorRequestUpdated
    {

      
        [Required(ErrorMessage = "Specialty can't be blank")]

        public string? Specalist { get; set; }
        [Required(ErrorMessage = "ClinicLocation can't be blank")]
        public string? Location { get; set; }
        public string? Description { set; get; }

        public decimal Price { get; set; }
        // [RegularExpression(@"^\+9627[789]\d{7}$", ErrorMessage = "Invalid phone number")]
        //   public string? Phone { get; set; }
        public string? ImageUrl { get; set; }
        public double? Rating { get; set; }
        public int ReviewCount { get; set; }
        public bool? IsAvailable { get; set; }



    }
}
