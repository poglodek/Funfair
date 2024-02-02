using Funfair.Dal.CosmosDb;
using Funfair.Dal.CosmosDb.Options;
using Funfair.Logging;
using Funfair.Messaging.AzureServiceBus;
using Funfair.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Core.Entities;
using Users.Core.Repositories;
using Users.Infrastructure.DAL.Container;
using Users.Infrastructure.DAL.Repositories;

namespace Users.Infrastructure;

public static class Extensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var cosmosContainerOptions = builder.Configuration.GetSection("CosmosDb").Get<ContainerOptions>();
        
        
        builder
          .AddAzureLogAnalytics()
          .AddMessageBus()
          .AddCosmosDb<UserContainer>(cosmosContainerOptions)
          .AddPipelineBehavior()
          .Services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddScoped<Middleware>();

        return builder;
    }
    

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<Middleware>();
        app.UseMessageBus();
        
        return app;
    }

}