using Confluent.Kafka;
using MongoDB.Bson;
using MongoConsumer.Common;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Models.Entities;
using MongoConsumer.Repositories.TelemetryRepository.Interfaces;
using MongoConsumer.Services.Kafka.Consumers.Interfaces;

namespace MongoConsumer.Services.Kafka.Consumers;

public class TelemetryConsumer : ITelemetryConsumer
{
    private readonly IConsumer<string, string> _kafkaConsumer;
    private readonly ITelemetryRepository _telemetryRepository;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TimeSpan _consumeTimeout;
    private readonly int _tailId;
    private readonly Task _consumeTask;
    private bool _isDisposed;

    public TelemetryConsumer(
        KafkaConsumerConfiguration configuration,
        int tailId,
        ITelemetryRepository telemetryRepository)
    {
        _tailId = tailId;
        _telemetryRepository = telemetryRepository;
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

        _consumeTask = Task.Run(ConsumeLoopAsync);
    }

    private async Task ConsumeLoopAsync()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                ConsumeResult<string, string>? result = _kafkaConsumer.Consume(_consumeTimeout);

                if (result?.Message != null)
                {
                    TelemetryDocument document = new TelemetryDocument
                    {
                        TailId = _tailId,
                        TelemetryData = BsonDocument.Parse(result.Message.Value),
                        Timestamp = DateTime.UtcNow
                    };

                    await _telemetryRepository.SaveAsync(document);
                }
            }
            catch (ConsumeException)
            {
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
    }
}
