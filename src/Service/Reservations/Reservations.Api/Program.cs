using Funfair.Auth;
using Funfair.KeyVault;
using Funfair.Messaging.EventHubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservations.Api;
using Reservations.App;
using Reservations.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .AddKeyVault()
    .AddApp()
    .AddInfrastructure()
    .AddEventHubs()
    .Services.AddAuth(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto
);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy( builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .Build();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseCors();
app.UseInfrastructure();


app.MapGet("/", () => Results.Ok($"Reservations working! - {DateTimeOffset.UtcNow}"));

EndPoints.Map(app);

app.UseSwaggerUI();
await app.RunAsync();