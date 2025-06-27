using DevLife_Portal.Common.Extensions;
using System.Text.Json;

namespace DevLife_Portal.Features.EscapeMeeting
{
    public class GetFavoriteExcuses
    {
        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("/escape/favorites", Handler).WithTags("EscapeMeeting");
            }
        }

        public static async Task<IResult> Handler(HttpContext context, RedisService redis)
        {
            var userId = context.Session.GetString("userId");
            if (string.IsNullOrWhiteSpace(userId)) return Results.Unauthorized();

            var key = $"favorites:{userId}";
            var raw = await redis.Db.ListRangeAsync(key);
            var favorites = raw.Select(r => JsonSerializer.Deserialize<SaveFavoriteExcuse.Request>(r!)).ToList();

            return Results.Ok(favorites);
        }
    }
}
