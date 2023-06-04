using System.Security.Cryptography.X509Certificates;
using System.Text;
using Disco.Shared.Auth.Exceptions;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Disco.Shared.Auth;

public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var options = configuration.GetSection("auth").Get<AuthOptions>();
        var key = ReturnSecurityKeyFromConfiguration(options);

        serviceCollection.AddSingleton(_ => options);
        
        serviceCollection.AddAuthentication(authenticationOptions =>
        {
            authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.RequireHttpsMetadata = false;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.JwtIssuer,
                    ValidAudience  = options.JwtIssuer,
                    IssuerSigningKey = key
                };
            });

        serviceCollection.AddSingleton<IJsonWebTokenManager, JsonWebTokenManager>();
        serviceCollection.AddSingleton<SecurityKeyCert>( _ => new (key));
        
        return serviceCollection;
    }

    private static SecurityKey? ReturnSecurityKeyFromConfiguration(AuthOptions options)
    {
        var certIsInConfig = options.Url is not null || options.Password is not null;
        var certExists = certIsInConfig && File.Exists(options.Url);
        var jwtKeyExists = options.JwtKey is not null;

        if (certExists)
        {
            return new X509SecurityKey(new X509Certificate2(options.Url!, options.Password!));
        }
        
        if (jwtKeyExists)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.JwtKey!));
        }

        throw new InvalidAuthConfigException("Auth config is invalid missing jwtKey or valid url with password");
        
        
    }

    public record SecurityKeyCert(SecurityKey Key);
}