using MongoConsumer.Services.TailIdStorage.Interfaces;
using MongoConsumer.Services.UAVChangeHandlers.Interfaces;

namespace MongoConsumer.Services.UAVChangeHandlers;

public class UAVDeletedHandler : IUAVChangeHandler
{
    private readonly ITailIdStorageService _tailIdStorage;

    public UAVDeletedHandler(ITailIdStorageService tailIdStorage)
    {
        _tailIdStorage = tailIdStorage;
    }

    public Task HandleAsync(int tailId, int? newTailId = null, CancellationToken cancellationToken = default)
    {
        _tailIdStorage.RemoveTailId(tailId);
        return Task.CompletedTask;
    }
}
