using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.NotificationSystemAdapter.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
