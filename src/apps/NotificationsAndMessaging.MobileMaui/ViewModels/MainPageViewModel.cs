using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using NotificationsAndMessaging.MobileMaui.Services.NotificationHubFuns;

namespace NotificationsAndMessaging.MobileMaui.ViewModels
{
    public partial class MainPageViewModel
        : ObservableObject
    {
        private readonly ILogger<MainPageViewModel> _logger;
        private readonly DeviceRegistrationService _deviceRegistrationService;
        private readonly IDeviceInstallationService _deviceInstallationService;

        public MainPageViewModel(ILogger<MainPageViewModel> logger, IDeviceInstallationService deviceInstallationService, DeviceRegistrationService deviceRegistrationService)
        {
            _logger = logger;
            _deviceRegistrationService = deviceRegistrationService;
            _deviceInstallationService = deviceInstallationService;

            Init();
        }

        partial void Init();

        [ObservableProperty]
        private string _notificationMessage;

        [ObservableProperty]
        private string _tags;
    }
}
