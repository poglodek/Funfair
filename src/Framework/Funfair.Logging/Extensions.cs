using Funfair.Logging.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace Funfair.Logging;

public static class Extensions
{
    public static WebApplicationBuilder AddAzureLogAnalytics(this WebApplicationBuilder builder)
    {
        var options = builder.Configuration.GetSection("AzureLogAnalytics").Get<LogOptions>();

        if (options is null || !options.Enabled)
        {
            return builder;
        }

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            throw new ArgumentNullException($"{nameof(options.ConnectionString)} is null!");
        }
        
        if (string.IsNullOrWhiteSpace(options.InstrumentationKey))
        {
            throw new ArgumentNullException($"{nameof(options.InstrumentationKey)} is null!");
        }

        builder.Services.AddApplicationInsightsTelemetry(options.InstrumentationKey);
        
        builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Warning);

        
        builder.Logging
            .AddApplicationInsights(configureTelemetryConfiguration: (config) =>
                {
                    config.ConnectionString = options.ConnectionString;
                },
                configureApplicationInsightsLoggerOptions: _ => { });
        
        return builder;
    }
}