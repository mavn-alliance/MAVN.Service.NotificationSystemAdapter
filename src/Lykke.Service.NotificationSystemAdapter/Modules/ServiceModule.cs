using Autofac;
using JetBrains.Annotations;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.Dictionaries.Client;
using Lykke.Service.NotificationSystemAdapter.Infrastructure;
using Lykke.Service.NotificationSystemAdapter.Infrastructure.Authentication;
using Lykke.Service.NotificationSystemAdapter.Settings;
using Lykke.Service.PushNotifications.Client;
using Lykke.SettingsReader;

namespace Lykke.Service.NotificationSystemAdapter.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(
                new DomainServices.AutofacModule(
                    _appSettings.CurrentValue.NotificationSystemAdapterService.ImagesBaseUrl,
                    _appSettings.CurrentValue.Constants.TokenSymbol
                )
            );

            builder.RegisterCustomerProfileClient(_appSettings.CurrentValue.CustomerProfileService, null);

            builder.RegisterPushNotificationsClient(_appSettings.CurrentValue.PushNotificationsService, null);

            builder.RegisterDictionariesClient(_appSettings.CurrentValue.DictionariesService, null);

            builder.RegisterType<RequestContext>().As<IRequestContext>().InstancePerLifetimeScope();
            ApiAuthorizeAttribute.ValidApiKey = _appSettings.CurrentValue.NotificationSystemAdapterService.ApiKey;
        }
    }
}
