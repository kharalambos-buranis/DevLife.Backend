using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Services;
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

        public static async Task<IResult> Handler(HttpContext context, RedisService redis, CancellationToken cancellationToken)
        {
            var userId = context.Session.GetString("userId");
            if (string.IsNullOrWhiteSpace(userId)) return Results.Unauthorized();

            var raw = await redis.GetFavoritesAsync(int.Parse(userId), cancellationToken);
            var favorites = raw.Select(r => JsonSerializer.Deserialize<SaveFavoriteExcuse.Request>(r!)).ToList();

            return Results.Ok(favorites);
        }
    }
}
