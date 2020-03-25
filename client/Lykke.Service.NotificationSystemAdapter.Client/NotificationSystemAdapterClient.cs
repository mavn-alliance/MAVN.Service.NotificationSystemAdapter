using Lykke.HttpClientGenerator;

namespace Lykke.Service.NotificationSystemAdapter.Client
{
    /// <summary>
    /// NotificationSystemAdapter API aggregating interface.
    /// </summary>
    public class NotificationSystemAdapterClient : INotificationSystemAdapterClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to NotificationSystemAdapter Api.</summary>
        public INotificationSystemAdapterApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public NotificationSystemAdapterClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<INotificationSystemAdapterApi>();
        }
    }
}
