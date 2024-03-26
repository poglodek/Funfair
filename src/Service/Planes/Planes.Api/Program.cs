using Funfair.Auth;
using Funfair.KeyVault;
using Funfair.Messaging.EventHubs;
using Planes.App;
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

await app.RunAsync();
