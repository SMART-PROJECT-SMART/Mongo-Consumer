using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoConsumer.Common;
using MongoConsumer.Models.Configuration;
using MongoConsumer.Models.Entities;
using MongoConsumer.Repositories.Base;
using MongoConsumer.Repositories.TelemetryRepository.Interfaces;

namespace MongoConsumer.Repositories.TelemetryRepository;

public class TelemetryRepository : BaseRepository<TelemetryDocument>, ITelemetryRepository
{
    protected override string CollectionName => MongoConsumerConstants.Collections.TELEMETRY_COLLECTION;

    public TelemetryRepository(IMongoClient mongoClient, IOptions<MongoDbConfiguration> mongoDbConfig)
        : base(mongoClient, mongoDbConfig)
    {
    }
}
