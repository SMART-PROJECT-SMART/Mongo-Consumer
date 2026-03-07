using MongoConsumer.Services.Kafka.TelemetryConsumerManager.Interfaces;
using MongoConsumer.Services.UAVChangeHandlers.Interfaces;

namespace MongoConsumer.Services.UAVChangeHandlers;

public class UAVDeletedHandler : IUAVChangeHandler
{
    private readonly ITelemetryConsumerManager _consumerManager;

    public UAVDeletedHandler(ITelemetryConsumerManager consumerManager)
    {
        _consumerManager = consumerManager;
    }

    public Task HandleUAVChangeAsync(int tailId, int? newTailId = null, CancellationToken cancellationToken = default)
    {
        _consumerManager.RemoveConsumer(tailId);
        return Task.CompletedTask;
    }
}
