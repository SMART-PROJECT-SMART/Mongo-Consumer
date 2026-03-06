namespace MongoConsumer.Models.Configuration;

public class KafkaConsumerConfiguration
{
    public string BootstrapServers { get; set; }
    public string GroupIdPrefix { get; set; }
    public string TopicPrefix { get; set; }
}
