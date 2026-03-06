using MongoConsumer.Models.DTOs.DeviceManager;
using MongoConsumer.Services.Clients.DeviceManagerClient.Interfaces;
using MongoConsumer.Services.TailIdFetcher.Interfaces;
using MongoConsumer.Services.TailIdStorage.Interfaces;

namespace MongoConsumer.Services.TailIdFetcher;

public class TailIdFetcher : ITailIdFetcher
{
    private readonly ITailIdStorageService _tailIdStorage;
    private readonly IDeviceManagerClient _deviceManagerClient;

    public TailIdFetcher(
        ITailIdStorageService tailIdStorage,
        IDeviceManagerClient deviceManagerClient)
    {
        _tailIdStorage = tailIdStorage;
        _deviceManagerClient = deviceManagerClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        GetAllUAVsTailIdRo response = await _deviceManagerClient.GetAllUAVsTailId(cancellationToken);

        foreach (GetAllUAVsTailIdDto uav in response.UAVs)
        {
            _tailIdStorage.AddTailId(uav.TailId);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
