using MongoConsumer.Models.DTOs.Kafka;

namespace MongoConsumer.Services.Kafka.TelemetryConsumer.Interfaces;

public interface ITelemetryConsumer : IDisposable
{
    TelemetryDataDto? ConsumeTelemetryData(CancellationToken cancellationToken = default);
}
