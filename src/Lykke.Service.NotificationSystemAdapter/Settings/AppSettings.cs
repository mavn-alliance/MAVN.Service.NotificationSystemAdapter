using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.Dictionaries.Client;
using Lykke.Service.PushNotifications.Client;

namespace Lykke.Service.NotificationSystemAdapter.Settings
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
