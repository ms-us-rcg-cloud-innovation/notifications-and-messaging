using Microsoft.AspNetCore.Mvc;
using NotificationHub.Core.Builders.Interfaces;
using NotificationHub.Core.Models;
using NotificationHub.Core.Services;

namespace NotificationHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationHubService _notificationService;
        private readonly INotificationPayloadBuilder _notificationPayloadBuilder;

        public NotificationController(NotificationHubService notificationService, INotificationPayloadBuilder notificationPayloadBuilder)
        {
            _notificationService = notificationService;
            _notificationPayloadBuilder = notificationPayloadBuilder;
        }

        [HttpPost]
        public async Task<IActionResult> PushNotificationAsync(PushNotification notification)
        {
            try
            {
                var outcome = await _notificationService.SendNotificationAsync(notification.Platform, CreateRawPayload(notification));
                return Ok(outcome);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        private string CreateRawPayload(PushNotification notification)
        {
            _notificationPayloadBuilder
                .AddTitle(notification.Title)
                .AddBody(notification.Body);

            switch (notification.Platform)
            {
                case "fcm":
                    return _notificationPayloadBuilder.BuildAndroidPayload();
                case "aps":
                    return _notificationPayloadBuilder.BuildApplePayload();
                default:
                    throw new Exception("Invalid platform");
            }
        }
    }
}
