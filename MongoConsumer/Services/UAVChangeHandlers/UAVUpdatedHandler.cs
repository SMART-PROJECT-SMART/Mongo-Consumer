using MongoConsumer.Services.TailIdStorage.Interfaces;
using MongoConsumer.Services.UAVChangeHandlers.Interfaces;

namespace MongoConsumer.Services.UAVChangeHandlers;

public class UAVUpdatedHandler : IUAVChangeHandler
{
    private readonly ITailIdStorageService _tailIdStorage;

    public UAVUpdatedHandler(ITailIdStorageService tailIdStorage)
    {
        _tailIdStorage = tailIdStorage;
    }

    public Task HandleUAVChangeAsync(int tailId, int? newTailId = null, CancellationToken cancellationToken = default)
    {
        if (newTailId.HasValue && newTailId.Value != tailId)
        {
            _tailIdStorage.UpdateTailId(tailId, newTailId.Value);
        }

        return Task.CompletedTask;
    }
}
