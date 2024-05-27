using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RapidPay.DependencyInjection
{
    internal static class MediatR
    {
        public static void ConfigureMediatR(this IServiceCollection services)
        {
            var assemblies = new[]
            {
                Assembly.Load("RapidPay.Domain")
            };

            foreach (var assembly in assemblies)
            {
                services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly!));
            }
        }
    }
}
