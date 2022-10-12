using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.FunctionHelpers;
using NotificationHub.Core.Services;

namespace NotificationHub.MessagingFunctions.Functions
{
    public class RegisterDevice
    {
        private readonly ILogger _logger;
        private readonly NotificationHubService _hubService;

        public RegisterDevice(ILoggerFactory loggerFactory, NotificationHubService hubService)
        {
            _logger = loggerFactory.CreateLogger<RegisterDevice>();
            _hubService = hubService;
        }

        public record DeviceDetails(string DeviceHandle, NotificationPlatform Platform, IList<string> Tags, string Channel);

        [Function(nameof(RegisterDevice))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function
             , "post"
             , Route = "register-device")] HttpRequestData request)
        {
            var deviceDetails = await request.ReadFromJsonAsync<DeviceDetails>();

            if(deviceDetails is null)
            {
                return await request.CreateErrorResponseAsync("No content in request");
            }

            await _hubService.UpsertDeviceRegistrationAsync(deviceDetails.DeviceHandle, deviceDetails.Channel, deviceDetails.Platform, deviceDetails.Tags);

            return await request.CreateOkResponseAsync("Device registered!");
        }
    }
}
