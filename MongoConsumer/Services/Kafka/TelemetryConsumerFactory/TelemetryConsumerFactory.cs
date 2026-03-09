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
    private readonly ILoggerFactory _loggerFactory;

    public TelemetryConsumerFactory(
        IOptions<KafkaConsumerConfiguration> configuration,
        ITelemetryRepository telemetryRepository,
        ILoggerFactory loggerFactory
    )
    {
        _configuration = configuration.Value;
        _telemetryRepository = telemetryRepository;
        _loggerFactory = loggerFactory;
    }

    public ITelemetryConsumer CreateConsumer(int tailId)
    {
        ILogger<TelemetryConsumer> logger = _loggerFactory.CreateLogger<TelemetryConsumer>();
        return new TelemetryConsumer(_configuration, tailId, _telemetryRepository, logger);
    }
}
