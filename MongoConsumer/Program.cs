using MongoConsumer.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebApi().AddAppConfiguration(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
