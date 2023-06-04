using Funfair.KeyVault;
using Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructure();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseInfrastructure();

app.Run();