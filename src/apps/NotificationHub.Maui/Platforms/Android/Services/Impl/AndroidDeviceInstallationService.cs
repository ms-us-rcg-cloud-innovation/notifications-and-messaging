using Android.Gms.Common;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Android.Provider.Settings;

namespace NotificationHub.Maui.Platforms.Android.Services.Impl
{
    public class AndroidDeviceInstallationService
        : IDeviceInstallationService
    {
        private const string FIREBASE_TOKEN_KEY = "fcm-token";


        public async Task<string> GetTokenAsync()
            => await SecureStorage.GetAsync(FIREBASE_TOKEN_KEY);

        public async Task SetTokenAsync(string token)
            => await SecureStorage.SetAsync(FIREBASE_TOKEN_KEY, token);

        public async Task<DeviceInstallation> GenerateDeviceInstallationAsync(params string[] tags)
            => new DeviceInstallation
                {
                    Id = GetDeviceId(),
                    Platform = "fcm",
                    PushChannel = await GetTokenAsync(),
                    Tags = tags?.ToList()
                };

        public bool AreNotificationSupported(out string error)
        {
            error = null;
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(MainApplication.Context);
            bool isSuccess = resultCode != ConnectionResult.Success;

            if (!isSuccess)
            {
                error = GoogleApiAvailability.Instance.IsUserResolvableError(resultCode) ?
                   GoogleApiAvailability.Instance.GetErrorString(resultCode) :
                   "This device is not supported";
            }

            return isSuccess;
        }

        public string GetDeviceId()
        {
            var context = MainApplication.Context;

            var id = Secure.GetString(context.ContentResolver, Secure.AndroidId);

            return id;
        }

    }
}
