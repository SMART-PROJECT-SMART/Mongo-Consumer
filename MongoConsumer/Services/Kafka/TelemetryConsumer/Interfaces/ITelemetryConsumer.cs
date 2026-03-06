using Confluent.Kafka;

namespace MongoConsumer.Services.Kafka.TelemetryConsumer.Interfaces;

public interface ITelemetryConsumer : IDisposable
{
    ConsumeResult<string, string>? ConsumeTelemetryData();
}
