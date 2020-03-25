using JetBrains.Annotations;

namespace Lykke.Service.NotificationSystemAdapter.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class NotificationSystemAdapterSettings
    {
        public DbSettings Db { get; set; }

        public string ApiKey { get; set; }

        public string ImagesBaseUrl { get; set; }
    }
}
