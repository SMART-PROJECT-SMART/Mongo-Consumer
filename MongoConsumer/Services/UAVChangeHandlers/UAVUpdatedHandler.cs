using MongoConsumer.Services.Kafka.TelemetryConsumerManager.Interfaces;
using MongoConsumer.Services.UAVChangeHandlers.Interfaces;

namespace MongoConsumer.Services.UAVChangeHandlers;

public class UAVUpdatedHandler : IUAVChangeHandler
{
    private readonly ITelemetryConsumerManager _consumerManager;

    public UAVUpdatedHandler(ITelemetryConsumerManager consumerManager)
    {
        _consumerManager = consumerManager;
    }

    public Task HandleUAVChangeAsync(int tailId, int? newTailId = null, CancellationToken cancellationToken = default)
    {
        if (newTailId.HasValue && newTailId.Value != tailId)
        {
            _consumerManager.UpdateConsumer(tailId, newTailId.Value);
        }

        return Task.CompletedTask;
    }
}
