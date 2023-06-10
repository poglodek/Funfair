using Funfair.DAL.MsSql;
using Funfair.KeyVault;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Users.Core.Entities;
using Users.Infrastructure.DAL.DbContext;
using Users.Infrastructure.Query;

namespace Users.Infrastructure;

public static class Extensions
{
   public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
   {
      builder
         .AddAppByKeyVault("users")
         .AddMsSql<UserDbContext>()
         .AddGraphQl<User,UserSchema>();
      
      
      return builder;
   }

   public static WebApplication UseInfrastructure(this WebApplication app)
   {
      app.UseGraphQl();

      var scope = app.Services.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
      db.Database.Migrate();
      
      return app;
   }
   
}