using MongoConsumer.Services.Kafka.TelemetryConsumer.Interfaces;

namespace MongoConsumer.Services.Kafka.TelemetryConsumerFactory.Interfaces;

public interface ITelemetryConsumerFactory
{
    ITelemetryConsumer CreateConsumer(int tailId);
}
