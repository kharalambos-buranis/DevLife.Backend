﻿using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Services;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using Microsoft.AspNetCore.Mvc;

namespace DevLife_Portal.Features.Roast
{
    public class GetChallenge
    {
        public record Response(string Title, string Description, string Url);

        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("/api/roast/challenge", Handler)
                   .WithTags("Roast");
            }
        }

        public static async Task<IResult> Handler(
        HttpContext httpContext,
        AppDbContext context,
        ICodeWarsChallengeService codeWars,
        CancellationToken ct)
        {
            var userIdStr = httpContext.Session.GetString("userId");
            if (string.IsNullOrWhiteSpace(userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                return Results.Unauthorized();
            }

            var user = await context.Users.FindAsync(new object[] { userId }, ct);
            if (user is null)
            {
                return Results.Unauthorized();
            }

            var language = user.TechnoStack;
            var level = user.Experience;

            var challenge = await codeWars.GetRandomChallengeAsync(language, level, ct);
            if (challenge is null)
            {
                return Results.NotFound("No challenge found.");
            }

            return Results.Ok(new Response(
                challenge.Name,
                challenge.Description,
                challenge.Url + $" (Rank: {challenge.Rank.Name})"
            ));

        }
    }
}
