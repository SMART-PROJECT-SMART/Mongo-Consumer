using MongoConsumer.Common;
using MongoConsumer.Models.DTOs.DeviceManager;
using MongoConsumer.Services.Clients.DeviceManagerClient.Interfaces;

namespace MongoConsumer.Services.Clients.DeviceManagerClient
{
    public class DeviceManagerClient : IDeviceManagerClient
    {
        private readonly HttpClient _httpClient;

        public DeviceManagerClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(
                MongoConsumerConstants.HttpClients.DEVICE_MANAGER_HTTP_CLIENT
            );
        }

        public async Task<GetAllUAVsTailIdRo> GetAllUAVsTailId(
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                List<GetAllUAVsTailIdDto>? allUAVs =
                    await _httpClient.GetFromJsonAsync<List<GetAllUAVsTailIdDto>>(
                        MongoConsumerConstants.DeviceManagerApiEndpoints.GET_ALL_UAVS,
                        cancellationToken
                    );
                return new GetAllUAVsTailIdRo(allUAVs ?? []);
            }
            catch (HttpRequestException)
            {
                return new GetAllUAVsTailIdRo([]);
            }
        }
    }
}
