using Android.Gms.Common;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Services;
using Android.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Provider.Settings;

namespace NotificationHub.Maui.Platforms.Android.Services
{
    internal class AndroidDeviceInstallationService
        : IDeviceInstallationService
    {
        public string Token { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsNotificationsSupported(out string error)
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

        public DeviceInstallation GetDeviceInstallation(params string[] tags)
        {
            if (!IsNotificationsSupported(out string error))
            {
                throw new Exception(error);
            }

            if(string.IsNullOrEmpty(Token))
            {
                throw new Exception("No available token for FCM");
            }

            var installation = new DeviceInstallation
            {
                InstallationId = GetDeviceId(),
                Platform = "fcm",
                PushChannel = Token
            };

            return installation;
        }
    }
}
