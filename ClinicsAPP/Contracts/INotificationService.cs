using ClinicsAPP.Models;

namespace ClinicsAPP.Contracts
{
    public interface INotificationService
    {
        Task SendAsync(Guid userId, string title, string message, string type, int? relatedId = null);

        Task<List<Notification>> GetUserNotificationsAsync(Guid userId);

        Task MarkAsReadAsync(int notificationId);
    }
}
