namespace MongoConsumer.Models.DTOs.Kafka;

public class TelemetryDataDto
{
    public TelemetryDataDto(int tailId, string key, string value)
    {
        TailId = tailId;
        Key = key;
        Value = value;
    }

    public int TailId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}
