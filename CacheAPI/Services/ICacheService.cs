using CacheAPI.Models;

namespace CacheAPI.Services
{
    public interface ICacheService
    {
        Task<object?> GetAsync(string key);
        Task UpsertAsync(CacheEntryModel entry);
        Task UpsertAsync(CacheEntryWithExpirationOptionsModel entryWithExpirationOptions);
        Task RemoveAsync(string key);
        Task RemoveAllAsync();
        Task<object?> GetCacheStatisticsAsync();
    }
}
