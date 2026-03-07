using Confluent.Kafka;
using MongoConsumer.Common;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Services.Kafka.TelemetryConsumer.Interfaces;

namespace MongoConsumer.Services.Kafka.TelemetryConsumer;

public class TelemetryConsumer : ITelemetryConsumer
{
    private readonly IConsumer<string, string> _kafkaConsumer;
    private readonly ILogger<TelemetryConsumer> _logger;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TimeSpan _consumeTimeout;
    private readonly int _tailId;
    private readonly Task _consumeTask;
    private bool _isDisposed;

    public TelemetryConsumer(
        KafkaConsumerConfiguration configuration,
        int tailId,
        ILogger<TelemetryConsumer> logger)
    {
        _tailId = tailId;
        _logger = logger;
        _isDisposed = false;
        _cancellationTokenSource = new CancellationTokenSource();
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

        _consumeTask = Task.Factory.StartNew(
            ConsumeLoop,
            _cancellationTokenSource.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default
        );

        _logger.LogInformation("Consumer started for TailId={TailId}", _tailId);
    }

    private void ConsumeLoop()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                ConsumeResult<string, string>? result = _kafkaConsumer.Consume(_consumeTimeout);

                if (result?.Message != null)
                {
                    _logger.LogInformation(
                        "Consumed telemetry: TailId={TailId}, Key={Key}, Value={Value}",
                        _tailId,
                        result.Message.Key,
                        result.Message.Value
                    );
                }
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Consume error for TailId={TailId}", _tailId);
            }
            catch (OperationCanceledException)
            {
                break;
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

        _cancellationTokenSource.Cancel();

        try
        {
            _consumeTask.Wait();
        }
        catch (AggregateException)
        {
        }

        _cancellationTokenSource.Dispose();
        _kafkaConsumer.Close();
        _kafkaConsumer.Dispose();

        _logger.LogInformation("Consumer stopped for TailId={TailId}", _tailId);
    }
}
