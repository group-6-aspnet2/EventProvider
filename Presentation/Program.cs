using Buisness.Services;
using Data.Contexts;
using Data.Repositories;
using EventGrpcContract;
using Microsoft.EntityFrameworkCore;
using Presentation.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(x =>
{
    x.ListenAnyIP(5018, listenOption =>
    {
        listenOption.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });

    x.ListenAnyIP(7388, listenOption =>
    {
        listenOption.UseHttps();
        listenOption.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
});


builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration["ACS:ConnectionString"]));
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddTransient<Presentation.Services.EventGrpcService>();

builder.Services.AddGrpcClient<EventContract.EventContractClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:EventSettingUrl"]!);
});


var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGrpcService<EventGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
