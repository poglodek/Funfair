﻿using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Planes.App;

public static class Extensions
{
   public static WebApplicationBuilder AddApp(this WebApplicationBuilder builder)
   {
      builder.Services.AddMediatR(cfg =>  cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
      
      return builder;
   }

}