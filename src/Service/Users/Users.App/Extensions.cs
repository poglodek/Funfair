using System.Reflection;
using Funfair.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Users.App;

public static class Extensions
{
   public static WebApplicationBuilder AddApp(this WebApplicationBuilder builder)
   {
      builder.Services.AddMediatR(cfg =>  cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
      
      builder.AddDomain();
      return builder;
   }

}