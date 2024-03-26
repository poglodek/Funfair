using Funfair.Auth;
using Funfair.KeyVault;
using Funfair.Messaging.EventHubs;
using Planes.App;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

builder
    .AddKeyVault()
    .AddEventHubs()
    .AddApp()
    .Services.AddAuth(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


await app.RunAsync();
