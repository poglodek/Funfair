using Funfair.KeyVault;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppByKeyVault("users");


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();