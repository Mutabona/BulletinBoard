using Microsoft.Extensions.Caching.Distributed;

namespace BulletinBoard.Tests.AppServicesTests.Fake;

public interface ICacheService
{
    Task<string> GetStringAsync(string key, CancellationToken token);
    Task RemoveAsync(string key, CancellationToken token);
    Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken token);
}

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public Task<string> GetStringAsync(string key, CancellationToken token)
    {
        return _distributedCache.GetStringAsync(key, token);
    }

    public Task RemoveAsync(string key, CancellationToken token)
    {
        return _distributedCache.RemoveAsync(key, token);
    }

    public Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken token)
    {
        return _distributedCache.SetStringAsync(key, value, options, token);
    }
}
