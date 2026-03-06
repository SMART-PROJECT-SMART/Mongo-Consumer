using System.Text.Json.Serialization;

namespace MongoConsumer.Models.DTOs.DeviceManager;

public record GetAllUAVsTailIdDto([property: JsonPropertyName("tailId")] int TailId);
