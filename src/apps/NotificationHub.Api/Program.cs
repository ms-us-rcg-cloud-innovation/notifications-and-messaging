using Microsoft.Azure.NotificationHubs;
using NotificationHub.Core.Builders;
using NotificationHub.Core.Builders.Interfaces;
using NotificationHub.Core.Providers;
using NotificationHub.Core.Services;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add environment variables to our configuration provider
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add required abstraction layers for connecting to notification-hub
builder.Services.AddScoped<AzureNotificationProvider>();
builder.Services.AddScoped<INotificationPayloadBuilder, NotificationPayloadBuilder>();

builder.Services.AddScoped(sp =>
{
    // Retrieve values from configuration provider -- ours are stored in environment 
    // variables configured in our launchSettings.json (which is also in our .gitignore)
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

