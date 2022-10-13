using Android.Hardware.Usb;
using Android.Nfc;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Javax.Xml.Transform;
using NotificationHub.Core.Maui.Platforms.Android.Services.Impl;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsAzure.Messaging.NotificationHubs;
using AzNH = WindowsAzure.Messaging.NotificationHubs;

namespace NotificationHub.Maui.ViewModels
{
    public partial class MainPageViewModel
        : ObservableObject
    {
        private const string DEFAULT_INSTALLATION_TEMPLATE = "default";
        private InstallationTemplate _installationTemplate;

        partial void Init()
        {
            ChangeUsersCommand = new RelayCommand(SwapUsers);
            UpdateTagsCommand = new RelayCommand(UpdateTags);

            //AzNH.NotificationHub.SetUserId("Patrick");
            //AzNH.NotificationHub.InstallationId = "Patrick";

            //AzNH.NotificationHub.AddTag("user");
            
            //CurrentUser = AzNH.NotificationHub.UserId;
            //TagList = string.Join("; ", AzNH.NotificationHub.Tags.ToEnumerable<string>());

            //CurrentUser = AzNH.NotificationHub.UserId;
            //InstallationId = AzNH.NotificationHub.InstallationId;

            //var sysTags = AzNH.NotificationHub.Tags.ToEnumerable<string>();
            //var template = AzNH.NotificationHub.GetTemplate(DEFAULT_INSTALLATION_TEMPLATE);
            //if (template != null)
            //{
            //    TagList = string.Join("; ", template.Tags.ToEnumerable<string>());
            //}


            _notificationHandler = new DefaultAndroidNotificationHandler();
            _notificationHandler.NotificationReceived += (s, e) => { NotificationMessage = e.Message.Body; };
        }

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

        public ICommand ChangeUsersCommand { get; private set; }
        public ICommand UpdateTagsCommand { get; private set; }

        private void SwapUsers()
        {
            CurrentUser = UserId;
            InstallationId = CurrentUser;

            AzNH.NotificationHub.SetUserId(CurrentUser);
            AzNH.NotificationHub.InstallationId = CurrentUser;

            CrossFirebasePushNotification.Current.UnregisterForPushNotifications();
            CrossFirebasePushNotification.Current.RegisterForPushNotifications();

            var body = "{\"data\":{\"message\":\"$(message)\"}}";
            _installationTemplate = new InstallationTemplate();
            _installationTemplate.Body = body;
            
            var tags = new[] { "user" };
            _installationTemplate.AddTags(tags);

            AzNH.NotificationHub.SetTemplate(DEFAULT_INSTALLATION_TEMPLATE, _installationTemplate);

            AzNH.NotificationHub.Instance.InstanceInstallationSaved += (s, e) => Instance_InstanceInstallationSaved(s, e, tags);
            // swap installations
            AzNH.NotificationHub.BeginInstallationUpdate();

        }

        private void Instance_InstanceInstallationSaved(object sender, InstallationAdapterEventArgs e, string[] tags)
        {

            AzNH.NotificationHub.RemoveTags(AzNH.NotificationHub.Tags.ToEnumerable<string>().ToList());
            AzNH.NotificationHub.AddTags(tags);
            TagList = string.Join("; ", _installationTemplate.Tags.ToEnumerable<string>());

            var sysTags = AzNH.NotificationHub.Tags.ToEnumerable<string>();
        }

        private void UpdateTags()
        {
            var tags = Tags?.Split("; ") ?? new string[] { "" };
            _installationTemplate.AddTags(tags);
            AzNH.NotificationHub.AddTags(tags);
            AzNH.NotificationHub.BeginInstallationUpdate();

        }
    }
}
