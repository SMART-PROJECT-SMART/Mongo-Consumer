namespace MongoConsumer.Repositories.Base.Interfaces;

public interface IRepository<T> where T : class
{
    Task SaveAsync(T document, CancellationToken cancellationToken = default);
}
