using Funfair.DAL.MsSql;
using Funfair.KeyVault;
using Microsoft.AspNetCore.Builder;
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
         .AddMsSql<IUserDbContext,UserDbContext>()
         .AddGraphQl<User,UserSchema>();
      
      
      return builder;
   }

   public static WebApplication UseInfrastructure(this WebApplication app)
   {
      app.UseGraphQl();

      var db = app.Services.GetRequiredService<UserDbContext>();
      db.Migration();
      
      return app;
   }
   
}