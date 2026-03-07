using MongoConsumer.Models.Entities;
using MongoConsumer.Repositories.Base.Interfaces;

namespace MongoConsumer.Repositories.TelemetryRepository.Interfaces;

public interface ITelemetryRepository : IRepository<TelemetryDocument>
{
}
