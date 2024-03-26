using Funfair.Auth;
using Funfair.KeyVault;
using Funfair.Messaging.EventHubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Planes.App;
using Planes.App.Commands;
using Planes.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

builder
    .AddKeyVault()
    .AddEventHubs()
    .AddApp()
    .AddInfrastructure()
    .Services.AddAuth(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseInfrastructure();

app.MapPost("/", async ([FromServices] IMediator mediator, [FromBody] CreatePlaneCommand planeCommand) =>
{
    await mediator.Send(planeCommand);
    return Results.Ok();
});

await app.RunAsync();
