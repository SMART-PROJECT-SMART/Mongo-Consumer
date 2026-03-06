using MongoConsumer.Models.DTOs.Kafka;

namespace MongoConsumer.Services.Kafka.TelemetryConsumerManager.Interfaces;

public interface ITelemetryConsumerManager : IDisposable
{
    void AddConsumer(int tailId);
    void RemoveConsumer(int tailId);
    void UpdateConsumer(int oldTailId, int newTailId);
    IEnumerable<TelemetryDataDto> ConsumeAllTelemetryData(CancellationToken cancellationToken = default);
}
