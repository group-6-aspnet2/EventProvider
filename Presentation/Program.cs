using Buisness.Services;
using Data.Contexts;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Presentation.Services;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddGrpc();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v.1.0",
        Title = "Event Service API Documentation",
        Description = "Official documentation for Event Service Provider API."
    });

    o.EnableAnnotations();
    o.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(x =>
    {
        x.ListenAnyIP(8585, listenOption =>
        {
            listenOption.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        });

        x.ListenAnyIP(7388, listenOption =>
        {
            listenOption.UseHttps();
            listenOption.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
        });
    });
}


builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration["ACS:ConnectionString"]));
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddTransient<EventGrpcService>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event API V1");
    c.RoutePrefix = string.Empty; 
});

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGrpcService<EventGrpcService>();
app.MapGet("/grpc", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
