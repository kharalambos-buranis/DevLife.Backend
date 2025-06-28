using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System.Text.Json;
using IDatabase = StackExchange.Redis.IDatabase;

namespace DevLife_Portal.Common.Services
{
    public class RedisService
    {
        private readonly IDatabase _db;

        public RedisService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task AddFavoriteAsync(int userId, string excuse, CancellationToken ct = default)
        {
            string key = $"favorites:{userId}";
            await _db.SetAddAsync(key, excuse);
        }

        public async Task<List<string>> GetFavoritesAsync(int userId, CancellationToken ct = default)
        {
            string key = $"favorites:{userId}";
            var values = await _db.SetMembersAsync(key);
            return values.Select(x => x.ToString()).ToList();
        }

        public void AddToQueue<T>(string key, T request)
        {
            _db.ListRightPush(key, JsonSerializer.Serialize(request), flags: CommandFlags.FireAndForget);
        }
    }
}
