using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RapidPay.Domain.Data;

namespace RapidPay.Domain.Payment.Hosted;

public class BalanceUpdateHostedService : IHostedLifecycleService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IServiceScopeFactory _scopeFactory;

    public BalanceUpdateHostedService(IMemoryCache memoryCache, IServiceScopeFactory scopeFactory)
    {
        _memoryCache = memoryCache;
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task StartedAsync(CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var groupedBalance = await dbContext.Purchases
            .GroupBy(x => x.CreditCardId).Select(x => new
            {
                CreditCardId = x.Key,
                TotalAmount = x.Sum(x => x.TotalPrice)
            })
            .ToListAsync();

            foreach (var balance in groupedBalance)
            {
                _memoryCache.Set($"balance_{balance.CreditCardId}", balance.TotalAmount);
            }
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
