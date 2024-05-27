using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RapidPay.Domain.Data;
using RapidPay.Domain.Infrastructure.Attributes;
using RapidPay.Domain.Store.Models;

namespace RapidPay.Domain.Store.Services;

[AddDependency]
public class FeeService : IFeeService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IApplicationDbContext _context;

    public FeeService(IMemoryCache memoryCache, IApplicationDbContext context)
    {
        _memoryCache = memoryCache;
        _context = context;
    }

    public async Task CalculateFeeAsync()
    {
        var fee = await _context.Fees
            .OrderByDescending(x => x.ReferenceDate)
            .Take(1)
            .SingleOrDefaultAsync();

        if (fee == null)
        {
            fee = new Fee
            {
                Past = 1,
                Current = CalculateUFE(),
                ReferenceDate = DateTime.UtcNow,
            };

            await _context.Fees.AddAsync(fee);
            await _context.SaveChangesAsync();
        }

        if (fee.ReferenceDate.Hour != DateTime.UtcNow.Hour)
        {
            var newFee = new Fee
            {
                Past = fee.Current,
                Current = CalculateUFE(),
                ReferenceDate = DateTime.UtcNow
            };

            await _context.Fees.AddAsync(newFee);
            await _context.SaveChangesAsync();

            fee = newFee;
        }

        _memoryCache.Set("fee", fee.Past * fee.Current);
    }

    public decimal GetFee()
    {
        var fee = _memoryCache.Get("fee") ?? 0M;
        return (decimal)fee;
    }

    private decimal CalculateUFE()
    {
        var random = new Random();
        return (decimal)random.NextDouble() * 2;
    }
}
