using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoConsumer.Common;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Repositories.TelemetryRepository;
using MongoConsumer.Repositories.TelemetryRepository.Interfaces;
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
            services.Configure<MongoDbConfiguration>(
                configuration.GetSection(
                    MongoConsumerConstants.Configuration.MONGODB_CONFIG_SECTION
                )
            );
            return services;
        }

        public static IServiceCollection AddMongoDbServices(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                MongoDbConfiguration config = sp.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;
                return new MongoClient(config.ConnectionString);
            });
            services.AddSingleton<ITelemetryRepository, TelemetryRepository>();
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

        public static IServiceCollection AddKafkaServices(this IServiceCollection services)
        {
            services.AddSingleton<ITelemetryConsumerFactory, TelemetryConsumerFactory>();
            services.AddSingleton<ITelemetryConsumerManager, TelemetryConsumerManager>();
            return services;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddSingleton<ITailIdFetcher, TailIdFetcher>();
            services.AddHostedService(sp => sp.GetRequiredService<ITailIdFetcher>());
            return services;
        }

        public static IServiceCollection AddUAVChangeHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IUAVChangeHandlerFactory, UAVChangeHandlerFactory>();
            return services;
        }
    }
}
