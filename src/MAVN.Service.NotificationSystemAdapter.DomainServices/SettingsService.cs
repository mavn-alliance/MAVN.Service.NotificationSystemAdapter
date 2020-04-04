using MAVN.Service.NotificationSystemAdapter.Domain.Services;

namespace MAVN.Service.NotificationSystemAdapter.DomainServices
{
    public class SettingsService : ISettingsService
    {
        private readonly string _tokenSymbol;

        public SettingsService(
            string tokenSymbol
        )
        {
            _tokenSymbol = tokenSymbol;
        }

        public string GetTokenSymbol()
            => _tokenSymbol;
    }
}
