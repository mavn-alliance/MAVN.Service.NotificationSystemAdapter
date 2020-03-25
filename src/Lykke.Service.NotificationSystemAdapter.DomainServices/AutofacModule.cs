using Autofac;
using Lykke.Service.NotificationSystemAdapter.Domain.Services;

namespace Lykke.Service.NotificationSystemAdapter.DomainServices
{
    public class AutofacModule : Module
    {
        private readonly string _imagesBaseUrl;
        private readonly string _tokenSymbol;

        public AutofacModule(
            string imagesBaseUrl,
            string tokenSymbol
        )
        {
            _imagesBaseUrl = imagesBaseUrl;
            _tokenSymbol = tokenSymbol;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<KeysService>()
                .As<IKeysService>()
                .WithParameter("imagesBaseUrl", _imagesBaseUrl)
                .SingleInstance();

            builder.RegisterType<SettingsService>()
                .WithParameter("tokenSymbol", _tokenSymbol)
                .As<ISettingsService>()
                .SingleInstance();
        }
    }
}
