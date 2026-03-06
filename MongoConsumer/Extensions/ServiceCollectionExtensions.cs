using System.Runtime.CompilerServices;
using MongoConsumer.Common;
using MongoConsumer.Models.Configuration;

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
        }
    }
}
