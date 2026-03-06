using System.Runtime.CompilerServices;
using MongoConsumer.Common;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Services.Clients.DeviceManagerClient;
using MongoConsumer.Services.Clients.DeviceManagerClient.Interfaces;

namespace MongoConsumer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            return services;
        }

        public static IServiceCollection AddAppConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<DeviceManagerConfiguration>(
                configuration.GetSection(
                    MongoConsumerConstants.Configuration.DEVICE_MANAGER_CONFIG_SECTION
                )
            );
            return services;
        }

        public static IServiceCollection AddHTTPClients(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            DeviceManagerConfiguration deviceManagerConfiguration = configuration
                .GetSection(MongoConsumerConstants.Configuration.DEVICE_MANAGER_CONFIG_SECTION)
                .Get<DeviceManagerConfiguration>()!;
            services.AddHttpClient(
                MongoConsumerConstants.HttpClients.DEVICE_MANAGER_HTTP_CLIENT,
                client =>
                {
                    client.BaseAddress = new Uri(deviceManagerConfiguration.BaseUrl);
                }
            );
            services.AddSingleton<IDeviceManagerClient, DeviceManagerClient>();
            return services;
        }
    }
}
