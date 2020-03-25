using System;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.NotificationSystemAdapter.Infrastructure;
using Lykke.Service.NotificationSystemAdapter.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.NotificationSystemAdapter
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "NotificationSystemAdapter API",
            ApiVersion = "v1"
        };

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<ApiKeyHeaderOperationFilter>();
            });

            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "NotificationSystemAdapterLog";
                    logs.AzureTableConnectionStringResolver = settings => settings.NotificationSystemAdapterService.Db.LogsConnString;
                };
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration(options =>
            {
                options.SwaggerOptions = _swaggerOptions;
            });
        }
    }
}
