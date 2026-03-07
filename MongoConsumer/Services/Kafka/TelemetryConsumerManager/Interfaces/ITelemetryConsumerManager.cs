namespace MongoConsumer.Services.Kafka.TelemetryConsumerManager.Interfaces;

public interface ITelemetryConsumerManager : IDisposable
{
    void AddConsumer(int tailId);
    void RemoveConsumer(int tailId);
    void UpdateConsumer(int oldTailId, int newTailId);
}
