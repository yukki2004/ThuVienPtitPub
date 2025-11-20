using StackExchange.Redis;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Infrastructure.Respository
{

    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;
            return System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiry);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
        public async Task<int?> GetVersionAsync(string key)
        {
            var val = await _db.StringGetAsync(key);
            if (val.IsNullOrEmpty) return null;
            if (int.TryParse(val, out int version))
                return version;
            return null;
        }

        public async Task SetVersionAsync(string key, int version)
        {
            await _db.StringSetAsync(key, version);
        }
    }
}

