using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RapidPay.Domain.Store.Services;

namespace RapidPay.Domain.Store.Hosted;

public class FeeUpdateHostedService : IHostedLifecycleService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public FeeUpdateHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task StartedAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var feeService = scope.ServiceProvider.GetRequiredService<IFeeService>();
                await feeService.CalculateFeeAsync();
            }

            // should run each hour in the first second
            var utcNow = DateTime.UtcNow;
            var nextRun = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour + 1, 0, 1);
            var timeDiff = nextRun - utcNow;

            await Task.Delay(timeDiff, cancellationToken);
        }
    }

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
