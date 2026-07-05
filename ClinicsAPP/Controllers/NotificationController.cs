using ClinicsAPP.Contracts;
using ClinicsAPP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClinicsAPP.Controllers
{
    [Route("Notifications")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            // if user not authenticated return empty partial
            if (!User.Identity?.IsAuthenticated ?? true)
                return PartialView("~/Views/Notification/_NotificationsPartial.cshtml", Enumerable.Empty<Notification>());

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userGuid))
            {
                return PartialView("~/Views/Notification/_NotificationsPartial.cshtml", Enumerable.Empty<Notification>());
            }

            var notifications = await _notificationService.GetUserNotificationsAsync(userGuid);

            return PartialView("~/Views/Notification/_NotificationsPartial.cshtml", notifications);
        }
    }
}
