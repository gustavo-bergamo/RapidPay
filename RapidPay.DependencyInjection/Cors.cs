using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RapidPay.DependencyInjection;

internal static class Cors
{
    public static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOrigins = configuration.GetSection("Cors").Get<string[]>();

        services.AddCors(options => options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins(corsOrigins);

            builder.WithMethods("OPTIONS", "HEAD", "GET", "POST", "PUT");

            builder.AllowAnyHeader();

            builder.AllowCredentials();
        }));
    }
}
