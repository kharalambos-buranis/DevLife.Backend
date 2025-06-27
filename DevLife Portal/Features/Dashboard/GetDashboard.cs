using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevLife_Portal.Features.Dashboard
{
    public class GetDashboard
    {
        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("api/dashboard", Handler).WithTags("Users");
            }
        }

        public static async Task<IResult> Handler(
            HttpContext httpContext,
            AppDbContext context )
        {
            var userId = httpContext.Session.GetString("userId");

                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
                    return Results.Unauthorized();

                var user = await context.Users
                    .Include(u => u.ZodiacSign)
                    .FirstOrDefaultAsync(u => u.Id == parsedUserId);

                if (user is null)
                    return Results.NotFound("User not found");

                var zodiacPrediction = $"Today, {user.ZodiacSign.Name}s are energized and focused!";
                var techTips = new[] { "Refactor often", "Write tests", "Use Git", "Name variables well" };
                var luckyTechs = new[] { "React", "Docker", "Python", "Rust", "TypeScript" };

                var rnd = new Random();

                return Results.Ok(new
                {
                    message = $"Welcome back, {user.Name}!",
                    zodiac = $"{user.ZodiacSign.Name} {user.ZodiacSign.Emoji}",
                    prediction = zodiacPrediction,
                    techTip = techTips[rnd.Next(techTips.Length)],
                    luckyTechnology = luckyTechs[rnd.Next(luckyTechs.Length)],
                    navigation = new[] { "Start Challenge", "Leaderboard", "Profile" }
                });
            
        }
    }
}
