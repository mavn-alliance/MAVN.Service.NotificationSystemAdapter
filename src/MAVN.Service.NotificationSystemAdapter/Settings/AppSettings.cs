using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using MAVN.Service.CustomerProfile.Client;
using MAVN.Service.Dictionaries.Client;
using MAVN.Service.PushNotifications.Client;

namespace MAVN.Service.NotificationSystemAdapter.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public NotificationSystemAdapterSettings NotificationSystemAdapterService { get; set; }

        public ConstantsSettings Constants { get; set; }

        public CustomerProfileServiceClientSettings CustomerProfileService { get; set; }

        public PushNotificationsServiceClientSettings PushNotificationsService { get; set; }

        public DictionariesServiceClientSettings DictionariesService { get; set; }
    }
}
