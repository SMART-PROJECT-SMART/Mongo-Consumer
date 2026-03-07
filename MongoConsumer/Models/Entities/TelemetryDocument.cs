using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoConsumer.Models.Entities;

public class TelemetryDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRequired]
    public int TailId { get; set; }

    [BsonRequired]
    public BsonDocument TelemetryData { get; set; } = new();

    [BsonRequired]
    public DateTime Timestamp { get; set; }
}
