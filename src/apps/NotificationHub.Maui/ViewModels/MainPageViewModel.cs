using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using NotificationHub.Maui.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Maui.ViewModels
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
        private string _userId;

        [ObservableProperty]
        private string _installationId;

        [ObservableProperty]
        private string _tags;

        [ObservableProperty]
        private string _tagList;

        [ObservableProperty]
        private string _currentUser;
    }
}
