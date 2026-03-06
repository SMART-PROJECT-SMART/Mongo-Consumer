using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Models.DTOs.Kafka;
using MongoConsumer.Services.Kafka.TelemetryConsumer.Interfaces;
using MongoConsumer.Services.Kafka.TelemetryConsumerManager.Interfaces;

namespace MongoConsumer.Services.Kafka.TelemetryConsumerManager;

public class TelemetryConsumerManager : ITelemetryConsumerManager
{
    private readonly ConcurrentDictionary<int, ITelemetryConsumer> _consumers;
    private readonly KafkaConsumerConfiguration _configuration;
    private bool _isDisposed;

    public TelemetryConsumerManager(IOptions<KafkaConsumerConfiguration> configuration)
    {
        _configuration = configuration.Value;
        _consumers = new ConcurrentDictionary<int, ITelemetryConsumer>();
        _isDisposed = false;
    }

    public void AddConsumer(int tailId)
    {
        if (_isDisposed)
        {
            return;
        }

        _consumers.GetOrAdd(tailId, key =>
        {
            ITelemetryConsumer newConsumer = new TelemetryConsumer.TelemetryConsumer(_configuration, key);
            return newConsumer;
        });
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

    public IEnumerable<TelemetryDataDto> ConsumeAllTelemetryData(CancellationToken cancellationToken = default)
    {
        if (_isDisposed)
        {
            yield break;
        }

        foreach (KeyValuePair<int, ITelemetryConsumer> consumerEntry in _consumers)
        {
            ITelemetryConsumer consumer = consumerEntry.Value;
            TelemetryDataDto? telemetryData = consumer.ConsumeTelemetryData(cancellationToken);

            if (telemetryData != null)
            {
                yield return telemetryData;
            }
        }
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
            try
            {
                consumer.Dispose();
            }
            catch
            {
            }
        }

        _consumers.Clear();
    }
}
