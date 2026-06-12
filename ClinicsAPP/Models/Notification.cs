using System.ComponentModel.DataAnnotations;

namespace ClinicsAPP.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public Guid UserId { get; set; } // المريض أو الدكتور

        public string Title { get; set; }

        public string ?Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? Type { get; set; } // Booking / Cancel

        public int? RelatedId { get; set; } // مثلا BookingId
    }
}
