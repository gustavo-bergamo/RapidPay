using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidPay.Data;
using RapidPay.DependecyInjection.Errors;
using RapidPay.DependencyInjection;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RapidPay.DependecyInjection;

public static class DependencyInitializer
{
    public static void Initializer(IServiceCollection services, IConfiguration configuration)
    {
        Contract.Assert(services != null);
        Contract.Assert(configuration != null);

        services.AddMemoryCache();

        services.AddCorsConfiguration(configuration);

        services.AddExceptionHandlers();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("RapidPay"));
            options.EnableSensitiveDataLogging(true);
        });

        services.AddControllers().AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.AddIdentityConfigurations(configuration);
        services.AddAttributedDependencies();
        services.ConfigureSwagger();
        services.AddAutoMapper(typeof(AutomapperProfile));
        services.ConfigureMediatR();
        services.RunHostedServices();
    }
}