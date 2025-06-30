using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace DevLife_Portal.Features.BugChase
{
    public class GetLeaderboard
    {
        public record LeaderboardEntry(string Username, int Score);

        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("/api/bugchase/leaderboard", Handler)
                   .WithTags("BugChase");
            }
        }

        public static async Task<IResult> Handler(AppDbContext context, CancellationToken ct)
        {
            var users = await context.Users.ToListAsync(ct);
            var scores = await context.BugChaseScores.ToListAsync(ct);

            var leaderboard = users
                .Select(user =>
                {
                    var topScore = scores
                        .Where(s => s.UserId == user.Id)
                        .OrderByDescending(s => s.Score)
                        .Select(s => s.Score)
                        .FirstOrDefault(); 

                    return new LeaderboardEntry(user.Username, topScore);
                })
                .OrderByDescending(entry => entry.Score)
                .Take(10)
                .ToList();

            return Results.Ok(leaderboard);
        }
    }
}


