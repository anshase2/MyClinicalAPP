namespace ClinicsAPP.Models
{
    public class Doctor
    {
        public string FullName { set; get; }
        public string Specalist { set; get; }
        public string Description { set; get; }
      
    
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
     
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}