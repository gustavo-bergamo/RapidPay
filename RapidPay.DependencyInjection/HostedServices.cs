using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RapidPay.Domain.Payment.Hosted;
using RapidPay.Domain.Store.Hosted;

namespace RapidPay.DependencyInjection;

internal static class HostedServices
{
    public static void RunHostedServices(this IServiceCollection services)
    {
        services.Configure<HostOptions>(x =>
        {
            x.ServicesStartConcurrently = true;
            x.ServicesStopConcurrently = false;
        });
        services.AddHostedService<BalanceUpdateHostedService>();
        services.AddHostedService<FeeUpdateHostedService>();
    }
}
