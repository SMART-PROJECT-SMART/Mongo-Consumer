using MongoConsumer.Common;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Services.Clients.DeviceManagerClient;
using MongoConsumer.Services.Clients.DeviceManagerClient.Interfaces;
using MongoConsumer.Services.Kafka.TelemetryConsumerFactory;
using MongoConsumer.Services.Kafka.TelemetryConsumerFactory.Interfaces;
using MongoConsumer.Services.Kafka.TelemetryConsumerManager;
using MongoConsumer.Services.Kafka.TelemetryConsumerManager.Interfaces;
using MongoConsumer.Services.TailIdFetcher;
using MongoConsumer.Services.TailIdFetcher.Interfaces;
using MongoConsumer.Services.UAVChangeHandlers;
using MongoConsumer.Services.UAVChangeHandlers.Interfaces;

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
            services.Configure<KafkaConsumerConfiguration>(
                configuration.GetSection(
                    MongoConsumerConstants.Kafka.KAFKA_CONFIG_SECTION
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

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ITelemetryConsumerFactory, TelemetryConsumerFactory>();
            services.AddSingleton<ITelemetryConsumerManager, TelemetryConsumerManager>();
            services.AddSingleton<ITailIdFetcher, TailIdFetcher>();
            services.AddHostedService(sp => sp.GetRequiredService<ITailIdFetcher>());
            services.AddSingleton<IUAVChangeHandlerFactory, UAVChangeHandlerFactory>();
            return services;
        }
    }
}
