namespace MongoConsumer.Models.DTOs.DeviceManager;

public class GetAllUAVsTailIdRo
{
    public GetAllUAVsTailIdRo(IReadOnlyList<GetAllUAVsTailIdDto> uavs)
    {
        UAVs = uavs;
    }

    public IReadOnlyList<GetAllUAVsTailIdDto> UAVs { get; set; }
}
