using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.NotificationSystemAdapter.Client 
{
    /// <summary>
    /// NotificationSystemAdapter client settings.
    /// </summary>
    public class NotificationSystemAdapterServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}

        /// <summary>Api key for accessing the service</summary>
        public string ApiKey { get; set; }
    }
}
