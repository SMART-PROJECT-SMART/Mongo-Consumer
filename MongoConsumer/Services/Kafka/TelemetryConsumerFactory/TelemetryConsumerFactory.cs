using Microsoft.Extensions.Options;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Services.Kafka.TelemetryConsumer;
using MongoConsumer.Services.Kafka.TelemetryConsumer.Interfaces;
using MongoConsumer.Services.Kafka.TelemetryConsumerFactory.Interfaces;

namespace MongoConsumer.Services.Kafka.TelemetryConsumerFactory;

public class TelemetryConsumerFactory : ITelemetryConsumerFactory
{
    private readonly KafkaConsumerConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public TelemetryConsumerFactory(
        IOptions<KafkaConsumerConfiguration> configuration,
        ILoggerFactory loggerFactory
    )
    {
        _configuration = configuration.Value;
        _loggerFactory = loggerFactory;
    }

    public ITelemetryConsumer CreateConsumer(int tailId)
    {
        ILogger<TelemetryConsumer.TelemetryConsumer> logger =
            _loggerFactory.CreateLogger<TelemetryConsumer.TelemetryConsumer>();

        return new TelemetryConsumer.TelemetryConsumer(_configuration, tailId, logger);
    }
}
