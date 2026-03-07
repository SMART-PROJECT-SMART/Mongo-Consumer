using MongoConsumer.Common.Enums;
using MongoConsumer.Services.Kafka.TelemetryConsumerManager.Interfaces;
using MongoConsumer.Services.UAVChangeHandlers.Interfaces;

namespace MongoConsumer.Services.UAVChangeHandlers;

public class UAVChangeHandlerFactory : IUAVChangeHandlerFactory
{
    private readonly ITelemetryConsumerManager _consumerManager;

    public UAVChangeHandlerFactory(ITelemetryConsumerManager consumerManager)
    {
        _consumerManager = consumerManager;
    }

    public IUAVChangeHandler CreateHandler(CrudOperation operation)
    {
        return operation switch
        {
            CrudOperation.Created => new UAVCreatedHandler(_consumerManager),
            CrudOperation.Updated => new UAVUpdatedHandler(_consumerManager),
            CrudOperation.Deleted => new UAVDeletedHandler(_consumerManager),
            _ => throw new ArgumentException($"Unsupported operation: {operation}", nameof(operation))
        };
    }
}
