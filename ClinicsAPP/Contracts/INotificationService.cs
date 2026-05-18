using ClinicsAPP.Models;

namespace ClinicsAPP.Contracts
{
    public interface INotificationService
    {
        Task SendAsync(string userId, string title, string message, string type, int? relatedId = null);

        Task<List<Notification>> GetUserNotificationsAsync(string userId);

        Task MarkAsReadAsync(int notificationId);
    }
}
