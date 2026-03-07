using Microsoft.Extensions.Options;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Services.Kafka.Consumers;
using MongoConsumer.Services.Kafka.Consumers.Interfaces;
using MongoConsumer.Services.Kafka.TelemetryConsumerFactory.Interfaces;

namespace MongoConsumer.Services.Kafka.TelemetryConsumerFactory;

public class TelemetryConsumerFactory : ITelemetryConsumerFactory
{
    private readonly KafkaConsumerConfiguration _configuration;

    public TelemetryConsumerFactory(IOptions<KafkaConsumerConfiguration> configuration)
    {
        _configuration = configuration.Value;
    }

    public ITelemetryConsumer CreateConsumer(int tailId)
    {
        return new TelemetryConsumer(_configuration, tailId);
    }
}
