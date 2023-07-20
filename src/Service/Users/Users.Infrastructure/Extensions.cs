using Funfair.DAL.MsSql;
using Funfair.KeyVault;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Users.Core.Entities;
using Users.Core.Repositories;
using Users.Infrastructure.DAL.DbContext;
using Users.Infrastructure.DAL.Repositories;
using Users.Infrastructure.Query;

namespace Users.Infrastructure;

public static class Extensions
{
   public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
   {
      builder
         .AddAppByKeyVault("users")
         .AddMsSql<UserDbContext>()
         .AddGraphQl<UserQuery>()
            .Services.AddScoped<IUserRepository,UserRepository>()
               .AddScoped<IPasswordHasher<User>,PasswordHasher<User>>()
         .AddScoped<Middleware>();
      
      

      return builder;
   }

   public static WebApplication UseInfrastructure(this WebApplication app)
   {
      app.UseMiddleware<Middleware>();
      
      app.UseGraphQl();
      var scope = app.Services.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
      db.Database.Migrate();
      
      return app;
   }
   
}