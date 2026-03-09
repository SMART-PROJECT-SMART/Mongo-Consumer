using Microsoft.Extensions.Options;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Repositories.TelemetryRepository.Interfaces;
using MongoConsumer.Services.Kafka.Consumers;
using MongoConsumer.Services.Kafka.Consumers.Interfaces;
using MongoConsumer.Services.Kafka.TelemetryConsumerFactory.Interfaces;

namespace MongoConsumer.Services.Kafka.TelemetryConsumerFactory;

public class TelemetryConsumerFactory : ITelemetryConsumerFactory
{
    private readonly KafkaConsumerConfiguration _configuration;
    private readonly ITelemetryRepository _telemetryRepository;

    public TelemetryConsumerFactory(
        IOptions<KafkaConsumerConfiguration> configuration,
        ITelemetryRepository telemetryRepository)
    {
        _configuration = configuration.Value;
        _telemetryRepository = telemetryRepository;
    }

    public ITelemetryConsumer CreateConsumer(int tailId)
    {
        return new TelemetryConsumer(_configuration, tailId, _telemetryRepository);
    }
}
