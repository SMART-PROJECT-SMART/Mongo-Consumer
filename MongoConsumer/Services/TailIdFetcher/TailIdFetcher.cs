using MongoConsumer.Models.DTOs.DeviceManager;
using MongoConsumer.Services.Clients.DeviceManagerClient.Interfaces;
using MongoConsumer.Services.Kafka.TelemetryConsumerManager.Interfaces;
using MongoConsumer.Services.TailIdFetcher.Interfaces;

namespace MongoConsumer.Services.TailIdFetcher;

public class TailIdFetcher : ITailIdFetcher
{
    private readonly ITelemetryConsumerManager _consumerManager;
    private readonly IDeviceManagerClient _deviceManagerClient;

    public TailIdFetcher(
        ITelemetryConsumerManager consumerManager,
        IDeviceManagerClient deviceManagerClient)
    {
        _consumerManager = consumerManager;
        _deviceManagerClient = deviceManagerClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        GetAllUAVsTailIdRo response = await _deviceManagerClient.GetAllUAVsTailId(cancellationToken);

        foreach (GetAllUAVsTailIdDto uav in response.UAVs)
        {
            _consumerManager.AddConsumer(uav.TailId);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
