using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using NotificationHub.Api.Models;
using NotificationHub.Api.Services;

namespace NotificationHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly NotificationHubService _hubService;

        public RegisterController(NotificationHubService hubService)
        {
            _hubService = hubService;
        }

        [HttpPut]
        public async Task<IActionResult> UpsertRegistrationAsync(DeviceRegistration deviceRegistration)
        {
            NotificationPlatform platform;

            switch(deviceRegistration.Platform)
            {
                case "fcm":
                    platform = NotificationPlatform.Fcm;
                    break;
                default:
                    return BadRequest("Invalid platform defined");

            }

            await _hubService.UpsertDeviceRegistrationAsync(deviceRegistration.DeviceId, deviceRegistration.PushChannel, platform, deviceRegistration.Tags);

            return Ok();
        }
    }
}
