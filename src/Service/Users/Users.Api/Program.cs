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



app.UseSwaggerUI();
await app.RunAsync();