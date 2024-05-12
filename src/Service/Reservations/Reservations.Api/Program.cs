using Funfair.Auth;
using Funfair.KeyVault;
using Funfair.Messaging.EventHubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

//POST

EndPoints.Map(app);

//GET

app.UseSwaggerUI();
await app.RunAsync();