using Funfair.Auth;
using Funfair.KeyVault;
using Funfair.Messaging.EventHubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.App;
using Users.App.Commands;
using Users.Infrastructure;

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


app.MapGet("/", () => Results.Ok($"Users' working! - {DateTime.UtcNow}"));


app.MapPost("/SignUp", async ([FromServices] IMediator mediator, [FromBody] AddUserCommand user) =>
{

    await mediator.Send(user);
    
    return Results.Ok();
});

app.MapPost("/SignIn", async ([FromServices] IMediator mediator, [FromBody] SignInCommand login) =>
{

    var token = await mediator.Send(login);
    
    return Results.Ok(token);
});



app.UseSwaggerUI();
await app.RunAsync();