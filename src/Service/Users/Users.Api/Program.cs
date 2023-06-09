using System.Reflection;
using Funfair.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.App;
using Users.App.Commands;
using Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .AddApp()
    .AddInfrastructure()
    .Services.AddAuth(builder.Configuration);


var app = builder.Build();

app.UseSwagger();

app.UseInfrastructure();

app.MapGet("/", () => "Hello World!");

app.MapPost("/SignUp", async ([FromServices] IMediator mediator, [FromBody] AddUser user) =>
{

    await mediator.Send(user);
    
    return Results.Ok();
});

app.MapPost("/SignIn", async ([FromServices] IMediator mediator, [FromBody] SignIn login) =>
{

    var token = await mediator.Send(login);
    
    return Results.Ok(token);
});
app.UseHttpsRedirection();


app.UseSwaggerUI();
await app.RunAsync();