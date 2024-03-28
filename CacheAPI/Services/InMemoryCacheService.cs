using CacheAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CacheAPI.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly MemoryCache _cache;
        private readonly SemaphoreSlim _semaphore;
        private readonly int _cacheSizeLimit;

        public InMemoryCacheService(int cacheSizeLimit)
        {
            _cacheSizeLimit = cacheSizeLimit;
            _cache = new MemoryCache(new MemoryCacheOptions()
            {
                SizeLimit = _cacheSizeLimit,
                TrackStatistics = true                
            });

            _semaphore = new SemaphoreSlim(1);
        }

        private MemoryCacheEntryOptions GetCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions().SetSize(1);
        }

        public async Task<object?> GetAsync(string key)
        {
            return await Task.Run(() =>
            {
                if (!_cache.TryGetValue(key, out object? entry))
                    throw new KeyNotFoundException($"{key} cache entry not found.");

                return entry;
            });
        }

        public async Task UpsertAsync(CacheEntryModel entry)
        {
            try
            {
                await _semaphore.WaitAsync();

                if (_cache.Count >= _cacheSizeLimit && !_cache.TryGetValue(entry.Key, out object? value))
                    throw new OutOfCacheSizeLimitException("cache entry cannot be added because its max cache limit reached.");

                _cache.Set(entry.Key, entry.Value, GetCacheEntryOptions());
            }
            catch
            {
                throw;
            }
            finally
            { _semaphore.Release(); }
        }

        public async Task UpsertAsync(CacheEntryWithExpirationOptionsModel entryWithExpirationOptions)
        {
            try
            {
                if (_cache.Count >= _cacheSizeLimit && !_cache.TryGetValue(entryWithExpirationOptions.Key, out object? value))
                    throw new OutOfCacheSizeLimitException("cache entry cannot be added because its max cache size limit reached.");

                await _semaphore.WaitAsync();

                var expTime = TimeSpan.FromMinutes(entryWithExpirationOptions.ExpirationInMinutes);
                var options = GetCacheEntryOptions();
                if (entryWithExpirationOptions.IsSlidingExpiration)
                    options.SetSlidingExpiration(expTime);
                else
                    options.SetAbsoluteExpiration(expTime);

                _cache.Set(entryWithExpirationOptions.Key, entryWithExpirationOptions.Value, options);
            }
            catch
            {
                throw;
            }
            finally
            { _semaphore.Release(); }
        }

        public async Task RemoveAsync(string key)
        {
            await Task.Run(() => _cache.Remove(key));
        }

        public async Task RemoveAllAsync()
        {
            await Task.Run(() => _cache.Clear());
        }

        public async Task<object?> GetCacheStatisticsAsync()
        {
            return await Task.Run(() => _cache.GetCurrentStatistics());
        }
    }
}
