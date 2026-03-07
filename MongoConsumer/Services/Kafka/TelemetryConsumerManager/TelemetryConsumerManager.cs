using System.Collections.Concurrent;
using MongoConsumer.Services.Kafka.TelemetryConsumer.Interfaces;
using MongoConsumer.Services.Kafka.TelemetryConsumerFactory.Interfaces;
using MongoConsumer.Services.Kafka.TelemetryConsumerManager.Interfaces;

namespace MongoConsumer.Services.Kafka.TelemetryConsumerManager;

public class TelemetryConsumerManager : ITelemetryConsumerManager
{
    private readonly ConcurrentDictionary<int, ITelemetryConsumer> _consumers;
    private readonly ITelemetryConsumerFactory _consumerFactory;
    private bool _isDisposed;

    public TelemetryConsumerManager(ITelemetryConsumerFactory consumerFactory)
    {
        _consumerFactory = consumerFactory;
        _consumers = new ConcurrentDictionary<int, ITelemetryConsumer>();
        _isDisposed = false;
    }

    public void AddConsumer(int tailId)
    {
        if (_isDisposed)
        {
            return;
        }

        _consumers.GetOrAdd(tailId, id => _consumerFactory.CreateConsumer(id));
    }

    public void RemoveConsumer(int tailId)
    {
        if (_consumers.TryRemove(tailId, out ITelemetryConsumer? consumer))
        {
            consumer.Dispose();
        }
    }

    public void UpdateConsumer(int oldTailId, int newTailId)
    {
        RemoveConsumer(oldTailId);
        AddConsumer(newTailId);
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        foreach (ITelemetryConsumer consumer in _consumers.Values)
        {
            consumer.Dispose();
        }

        _consumers.Clear();
    }
}
