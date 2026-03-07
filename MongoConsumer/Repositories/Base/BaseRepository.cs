using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Repositories.Base.Interfaces;

namespace MongoConsumer.Repositories.Base;

public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly IMongoCollection<T> _collection;

    protected abstract string CollectionName { get; }

    protected BaseRepository(IMongoClient mongoClient, IOptions<MongoDbConfiguration> mongoDbConfig)
    {
        IMongoDatabase database = mongoClient.GetDatabase(mongoDbConfig.Value.DatabaseName);
        _collection = database.GetCollection<T>(CollectionName);
    }

    public virtual async Task SaveAsync(T document, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }
}
