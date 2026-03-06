using MongoConsumer.Models.DTOs.DeviceManager;

namespace MongoConsumer.Services.Clients.DeviceManagerClient.Interfaces
{
    public interface IDeviceManagerClient
    {
        Task<GetAllUAVsTailIdRo> GetAllUAVsTailId(CancellationToken cancellationToken = default);
    }
}
