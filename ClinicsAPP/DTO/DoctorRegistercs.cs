namespace ClinicsAPP.DTO
{
    public class DoctorRegistercs
    {
      
            public string? Specalist { get; set; }
            public string? Location { get; set; }
            public decimal ?Price { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string? Description { set; get; }


    }
}
