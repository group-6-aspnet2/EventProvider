using EventGrpcContractClient;
using GrpcService2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("local")));

builder.Services.AddGrpcClient<EventContract.EventContractClient>(x =>
{
    x.Address = new Uri("https://cs-ventixeevent.azurewebsites.net/");
});


builder.Services.AddGrpc();

var app = builder.Build();
app.MapOpenApi();
app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

builder.WebHost.ConfigureKestrel(x =>
{
    x.ListenAnyIP(5102, listenOption =>
    {
        listenOption.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });

    x.ListenAnyIP(7050, listenOption =>
    {
        listenOption.UseHttps();
        listenOption.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
});

app.MapGet("/test", async ([FromServices] EventContract.EventContractClient grpcClient) =>
{
    var response = await grpcClient.CreateEventAsync(new CreateEventRequest
    {
        EventName = "TestEvent",
        EventCategoryName = "TestCategory",
        EventLocation = "Stockholm",
        EventDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
        EventTime = DateTime.UtcNow.ToString("HH:mm"),
        EventStatus = "Active",
        EventAmountOfGuests = 100
    });

    return Results.Ok(new
    {
        response.Succeeded,
        response.Message,
        response.EventId
    });
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
