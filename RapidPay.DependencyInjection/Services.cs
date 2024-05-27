using Microsoft.Extensions.DependencyInjection;
using RapidPay.Domain.Infrastructure.Attributes;
using RapidPay.Domain.Infrastructure.Constants;
using System.Reflection;

namespace RapidPay.DependecyInjection;

public static class Services
{
    public static void AddAttributedDependencies(this IServiceCollection services)
    {
        var assemblies = new[]
        {
            Assembly.GetEntryAssembly(),
            Assembly.Load("RapidPay.Domain"),
            Assembly.Load("RapidPay.Data")
        };
        Add(services, assemblies!, typeof(AddDependencyAttribute));
    }

    private static void Add(IServiceCollection services, Assembly[] assemblies, Type attributeType)
    {
        var implementations = assemblies.SelectMany(a => a.GetTypes()
        .Where(t => t.IsDefined(attributeType, false)));

        var result = implementations.Select(i =>
        {
            dynamic attribute = Convert.ChangeType(i.GetCustomAttribute(attributeType), attributeType);
            Type @interface = attribute?.Interface ?? i.GetInterfaces().Single();
            return new
            {
                Interface = @interface,
                implementation = i,
                attribute?.Type
            };
        });

        foreach (var r in result)
        {
            switch (r.Type)
            {
                case DependencyType.Singleton:
                    services.AddSingleton(r.Interface, r.implementation);
                    break;
                case DependencyType.Scoped:
                    services.AddScoped(r.Interface, r.implementation);
                    break;
                case DependencyType.Transient:
                    services.AddTransient(r.Interface, r.implementation);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
