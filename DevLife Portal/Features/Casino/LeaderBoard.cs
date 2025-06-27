using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevLife_Portal.Features.Casino
{
    public class LeaderBoard
    {
        public record Response(string Username, int TotalPoints, int Streak);

        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("api/casino/leaderboard", Handler).WithTags("Casino");
            }

            public static async Task<IResult> Handler(
                [FromServices] AppDbContext db,
                CancellationToken cancellationToken)
            {
                var topUsers = await db.Users
                    .OrderByDescending(u => u.TotalPoints)
                    .ThenByDescending(u => u.Streak)
                    .Take(10)
                    .Select(u => new Response(u.Username, u.TotalPoints, u.Streak))
                    .ToListAsync(cancellationToken);

                return Results.Ok(topUsers);
            }
        }
    }
}
