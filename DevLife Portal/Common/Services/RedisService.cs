using Microsoft.EntityFrameworkCore.Storage;

namespace DevLife_Portal.Common.Services
{
    public class RedisService
    {
        private readonly IDatabase _db;

        public RedisService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task AddFavoriteAsync(int userId, string excuse, CancellationToken ct)
        {
            string key = $"favorites:{userId}";
            await _db.SetAddAsync(key, excuse);
        }

        public async Task<List<string>> GetFavoritesAsync(int userId, CancellationToken ct)
        {
            string key = $"favorites:{userId}";
            var values = await _db.SetMembersAsync(key);
            return values.Select(x => x.ToString()).ToList();
        }
    }
}
