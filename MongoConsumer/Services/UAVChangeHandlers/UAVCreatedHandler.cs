using MongoConsumer.Services.Kafka.TelemetryConsumerManager.Interfaces;
using MongoConsumer.Services.UAVChangeHandlers.Interfaces;

namespace MongoConsumer.Services.UAVChangeHandlers;

public class UAVCreatedHandler : IUAVChangeHandler
{
    private readonly ITelemetryConsumerManager _consumerManager;

    public UAVCreatedHandler(ITelemetryConsumerManager consumerManager)
    {
        _consumerManager = consumerManager;
    }

    public Task HandleUAVChangeAsync(int tailId, int? newTailId = null, CancellationToken cancellationToken = default)
    {
        _consumerManager.AddConsumer(tailId);
        return Task.CompletedTask;
    }
}
