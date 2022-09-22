using Microsoft.AspNetCore.Mvc;
using NotificationHub.Api.Models;
using NotificationHub.Api.Providers;

namespace NotificationHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly AzureNotificationProvider _notificationProvider;

        public NotificationController(AzureNotificationProvider notificationProvider)
        {
            _notificationProvider = notificationProvider;
        }

        [HttpPost]
        public async Task<IActionResult> PushNotificationAsync(Notification notification)
        {
            try
            {
                var outcome = await _notificationProvider.SendNotification(notification);

                return Ok(outcome);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }

        }
    }
}
