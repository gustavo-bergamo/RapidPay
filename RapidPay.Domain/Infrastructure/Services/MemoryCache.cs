using Microsoft.Extensions.Caching.Memory;

namespace RapidPay.Domain.Infrastructure.Services;

//[AddDependency(Type = DependencyType.Singleton)]
internal class MemoryCache : IMemoryCache
{
    private MemoryCacheOptions _memoryCacheOptions;

    public MemoryCache(MemoryCacheOptions memoryCacheOptions)
    {
        _memoryCacheOptions = memoryCacheOptions;
    }

    public MemoryCache Cache { get; } = new MemoryCache(
        new MemoryCacheOptions
        {
            SizeLimit = 1024
        });

    public ICacheEntry CreateEntry(object key)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Remove(object key)
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue(object key, out object? value)
    {
        throw new NotImplementedException();
    }
}
