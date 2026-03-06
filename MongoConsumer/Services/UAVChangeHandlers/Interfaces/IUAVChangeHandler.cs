namespace MongoConsumer.Services.UAVChangeHandlers.Interfaces;

public interface IUAVChangeHandler
{
    Task HandleUAVChangeAsync(int tailId, int? newTailId = null, CancellationToken cancellationToken = default);
}
