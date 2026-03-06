using MongoConsumer.Common.Enums;
using MongoConsumer.Services.TailIdStorage.Interfaces;
using MongoConsumer.Services.UAVChangeHandlers.Interfaces;

namespace MongoConsumer.Services.UAVChangeHandlers;

public class UAVChangeHandlerFactory : IUAVChangeHandlerFactory
{
    private readonly ITailIdStorageService _tailIdStorage;

    public UAVChangeHandlerFactory(ITailIdStorageService tailIdStorage)
    {
        _tailIdStorage = tailIdStorage;
    }

    public IUAVChangeHandler CreateHandler(CrudOperation operation)
    {
        return operation switch
        {
            CrudOperation.Created => new UAVCreatedHandler(_tailIdStorage),
            CrudOperation.Updated => new UAVUpdatedHandler(_tailIdStorage),
            CrudOperation.Deleted => new UAVDeletedHandler(_tailIdStorage),
            _ => throw new ArgumentException($"Unsupported operation: {operation}", nameof(operation))
        };
    }
}
