using Microsoft.Azure.NotificationHubs;
using NotificationHub.Api.Services;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped(sp =>
{
    var hubName = builder.Configuration.GetValue<string>("NOTIFICATION_HUB_NAME");
    var connectionString = builder.Configuration.GetValue<string>("NOTIFICATION_HUB_CS");

    var client = new NotificationHubClient(connectionString, hubName);

    return new NotificationHubService(client);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

