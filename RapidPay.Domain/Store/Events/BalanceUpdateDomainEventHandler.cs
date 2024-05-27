using MediatR;
using Microsoft.Extensions.Caching.Memory;
using RapidPay.Domain.Store.Models;

namespace RapidPay.Domain.Store.Events;

internal sealed class BalanceUpdateDomainEventHandler : INotificationHandler<BalanceUpdateDomainEvent>
{
    private readonly IMemoryCache _memoryCache;

    public BalanceUpdateDomainEventHandler(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task Handle(BalanceUpdateDomainEvent notification, CancellationToken cancellationToken)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));

        var cacheKey = $"balance_{notification.CreditCardId}";
        if (_memoryCache.TryGetValue(cacheKey, out decimal cacheValue))
        {
            cacheValue += notification.Amount;
            _memoryCache.Set(cacheKey, cacheValue, cacheEntryOptions);
        }
        else
        {
            _memoryCache.Set(cacheKey, notification.Amount, cacheEntryOptions);
        }

        return Task.CompletedTask;
    }
}
