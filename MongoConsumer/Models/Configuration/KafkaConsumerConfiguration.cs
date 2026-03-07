namespace MongoConsumer.Models.Configuration;

public class KafkaConsumerConfiguration
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string GroupIdPrefix { get; set; } = string.Empty;
    public string TopicPrefix { get; set; } = string.Empty;
    public int ConsumeTimeoutMs { get; set; }
}
