using System.Text.Json.Serialization;

namespace MongoConsumer.Models.DTOs.DeviceManager;

public class GetAllUAVsTailIdDto
{
    public GetAllUAVsTailIdDto(int tailId)
    {
        TailId = tailId;
    }

    [JsonPropertyName("tailId")]
    public int TailId { get; set; }
}
