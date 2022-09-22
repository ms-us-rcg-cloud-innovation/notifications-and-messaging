using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using NotificationHub.Api.Models;
using NotificationHub.Api.Services;
using Notification = NotificationHub.Api.Models.Notification;

namespace NotificationHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationHubService _hubService;

        public NotificationController(NotificationHubService hubService)
        {
            _hubService = hubService;
        }

        [HttpPost]
        public async Task<IActionResult> PushNotificationAsync(Notification notification)
        {
            var outcome = await _hubService.SendPushNotification(NotificationPlatform.Fcm, notification.Title, notification.Message);

            return Ok(outcome);
        }
    }
}
