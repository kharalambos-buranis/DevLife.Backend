using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Models;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using System.Security.Claims;

namespace DevLife_Portal.Features.Casino
{
    public static class SubmitBet
    {
        public record Request(string Selected, int BetPoints) : IRequest<Response>;

        public record Response(bool IsCorrect, int TotalPoints, int NewStreak, string Message);

        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("api/casino/bet", Handler)
                   .WithTags("Casino");
            }

            public static async Task<IResult> Handler(
                [FromBody] Request request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken)
            {
                var result = await mediator.Send(request, cancellationToken);
                return Results.Ok(result);
            }
        }

        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly AppDbContext context;
            private readonly IMongoCollection<CodeSnippet> _snippets;
            private readonly IHttpContextAccessor _httpContext;
            private readonly IMemoryCache _cache;

            public Handler(AppDbContext db, IMongoDatabase mongo, IHttpContextAccessor context, IMemoryCache cache)
            {
                this.context = db;
                _snippets = mongo.GetCollection<CodeSnippet>("casino_snippets");
                _httpContext = context;
                _cache = cache;
            } 

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var userIdString = _httpContext.HttpContext?.Session.GetString("userId");

                if (string.IsNullOrWhiteSpace(userIdString))
                {
                    throw new Exception("User is not logged in or session expired.");
                }

                var userId = int.Parse(userIdString);
                var user = await context.Users.FindAsync(userId);

                if (user is null)
                {
                    throw new Exception("User not found");
                }

                if (!_cache.TryGetValue($"casino:{userId}:correct", out string correctOption))
                {
                    return new Response(false, user.TotalPoints, user.Streak, "No active snippet found. Please load one first.");
                }

                Console.WriteLine($"User.TechnoStack = {user.TechnoStack}");
                Console.WriteLine($"User.Experience = {user.Experience}");

                var snippet = await _snippets.Find(s =>
                   s.Language.ToLower() == user.TechnoStack.ToLower() &&
                   s.Experience.ToLower() == user.Experience.ToLower()).FirstOrDefaultAsync();

                if (snippet is null)
                {
                    throw new Exception("Code snippet not found");
                }

                var isCorrect = request.Selected.ToLower() == correctOption.ToLower();
                var pointChange = isCorrect ? request.BetPoints * 2 : -request.BetPoints;

                user.TotalPoints = Math.Max(0, user.TotalPoints + pointChange);
                user.Streak = isCorrect ? user.Streak + 1 : 0;

                await context.SaveChangesAsync(cancellationToken);

                return new Response(
                    IsCorrect: isCorrect,
                    TotalPoints: user.TotalPoints,
                    NewStreak: user.Streak,
                    Message: isCorrect ? "🎉 You guessed right!" : "😞 Wrong answer!"
                );
            }
        }
    }
}
