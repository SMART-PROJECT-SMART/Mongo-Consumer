using MongoConsumer.Extensions;
using MongoConsumer.Services.Clients.DeviceManagerClient.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddWebApi()
    .AddAppConfiguration(builder.Configuration)
    .AddHTTPClients(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var client = scope.ServiceProvider.GetRequiredService<IDeviceManagerClient>();
    var result = await client.GetAllUAVsTailId();
    app.Logger.LogInformation("Fetched {Count} UAV tail IDs", result.UAVs.Count);
    foreach (var uav in result.UAVs)
    {
        app.Logger.LogInformation("TailId: {TailId}", uav.TailId);
    }
}

app.Run();
