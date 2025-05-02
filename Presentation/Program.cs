using Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Presentation.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddGrpc();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("event_localDB")));
//builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration["ACS:ConnectionString"]));
builder.Services.AddTransient<Presentation.Services.EventGrpcService>();


var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGrpcService<Presentation.Services.EventGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
