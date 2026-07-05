using Microsoft.Identity.Client;

namespace ClinicsAPP.DTO
{
    public class DoctorSearchDTO
    {
        public string? DoctorName { get; set; }
        public string? Gender { get; set; }
        public string? Location { get; set; }
        public string? Specialty { get; set; }
        public string? SortBy { get; set; } // e.g., "Rating", "Price".
    }
}
