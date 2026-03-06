namespace MongoConsumer.Services.UAVChangeHandlers.Interfaces;

public interface IUAVChangeHandler
{
    Task HandleAsync(int tailId, int? newTailId = null, CancellationToken cancellationToken = default);
}
