using Autofac;
using JetBrains.Annotations;
using MAVN.Service.CustomerProfile.Client;
using MAVN.Service.Dictionaries.Client;
using MAVN.Service.NotificationSystemAdapter.Infrastructure;
using MAVN.Service.NotificationSystemAdapter.Infrastructure.Authentication;
using MAVN.Service.NotificationSystemAdapter.Settings;
using MAVN.Service.PushNotifications.Client;
using Lykke.SettingsReader;

namespace MAVN.Service.NotificationSystemAdapter.Modules
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
