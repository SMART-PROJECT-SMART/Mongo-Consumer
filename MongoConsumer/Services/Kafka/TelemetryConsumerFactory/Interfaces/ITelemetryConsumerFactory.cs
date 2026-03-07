using MongoConsumer.Services.Kafka.Consumers.Interfaces;

namespace MongoConsumer.Services.Kafka.TelemetryConsumerFactory.Interfaces;

public interface ITelemetryConsumerFactory
{
    ITelemetryConsumer CreateConsumer(int tailId);
}
