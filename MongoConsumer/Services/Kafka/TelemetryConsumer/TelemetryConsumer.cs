using Confluent.Kafka;
using MongoConsumer.Common;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Models.DTOs.Kafka;
using MongoConsumer.Services.Kafka.TelemetryConsumer.Interfaces;

namespace MongoConsumer.Services.Kafka.TelemetryConsumer;

public class TelemetryConsumer : ITelemetryConsumer
{
    private readonly IConsumer<string, string> _kafkaConsumer;
    private readonly CancellationTokenSource _disposeCts;
    private readonly TimeSpan _consumeTimeout;
    private readonly int _tailId;
    private bool _isDisposed;

    public TelemetryConsumer(KafkaConsumerConfiguration configuration, int tailId)
    {
        _isDisposed = false;
        _tailId = tailId;
        _disposeCts = new CancellationTokenSource();
        _consumeTimeout = TimeSpan.FromMilliseconds(configuration.ConsumeTimeoutMs);

        ConsumerConfig consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration.BootstrapServers,
            GroupId = $"{configuration.GroupIdPrefix}-tailId-{tailId}",
            AutoOffsetReset = AutoOffsetReset.Latest
        };

        _kafkaConsumer = new ConsumerBuilder<string, string>(consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

        string topicName = $"{configuration.TopicPrefix}{tailId}";
        TopicPartition topicPartition = new TopicPartition(
            topicName,
            new Partition(MongoConsumerConstants.Kafka.TELEMETRY_PARTITION)
        );
        _kafkaConsumer.Assign(new[] { topicPartition });
    }

    public TelemetryDataDto? ConsumeTelemetryData(CancellationToken cancellationToken = default)
    {
        if (_isDisposed)
        {
            return null;
        }

        using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            _disposeCts.Token
        );

        try
        {
            ConsumeResult<string, string>? result = _kafkaConsumer.Consume(_consumeTimeout);

            linkedCts.Token.ThrowIfCancellationRequested();

            if (result == null || result.Message == null)
            {
                return null;
            }

            return new TelemetryDataDto(_tailId, result.Message.Key, result.Message.Value);
        }
        catch (ConsumeException)
        {
            return null;
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        try
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
            _kafkaConsumer.Unassign();
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();
        }
        catch
        {
        }
    }
}
