using MongoConsumer.Services.TailIdStorage.Interfaces;
using MongoConsumer.Services.UAVChangeHandlers.Interfaces;

namespace MongoConsumer.Services.UAVChangeHandlers;

public class UAVCreatedHandler : IUAVChangeHandler
{
    private readonly ITailIdStorageService _tailIdStorage;

    public UAVCreatedHandler(ITailIdStorageService tailIdStorage)
    {
        _tailIdStorage = tailIdStorage;
    }

    public Task HandleUAVChangeAsync(int tailId, int? newTailId = null, CancellationToken cancellationToken = default)
    {
        _tailIdStorage.AddTailId(tailId);
        return Task.CompletedTask;
    }
}
