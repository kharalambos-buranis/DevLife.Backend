using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Models;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace DevLife_Portal.Features.Casino
{
    public class DailyChallenge
    {
        public record Request() : IRequest<Response>;

        public record Response(string A, string B, string Explanation, string Message);

        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("api/casino/daily", Handler).WithTags("Casino");
            }

            public static async Task<IResult> Handler(
                HttpContext httpContext,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken)
            {
                var result = await mediator.Send(new Request(), cancellationToken);
                return Results.Ok(result);
            }
        }

        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly AppDbContext context;
            private readonly IMongoCollection<CodeSnippet> _snippets;
            private readonly IHttpContextAccessor httpContextAccessor;
            private readonly IMemoryCache cache;

            public Handler(AppDbContext context, IMongoDatabase mongo, IHttpContextAccessor http, IMemoryCache cache)
            {
                this.context = context;
                _snippets = mongo.GetCollection<CodeSnippet>("casino_snippets");
                httpContextAccessor = http;
                this.cache = cache;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var userIdString = httpContextAccessor.HttpContext?.Session.GetString("userId");
                if (string.IsNullOrWhiteSpace(userIdString))
                    throw new Exception("Not logged in");

                var userId = int.Parse(userIdString);
                var user = await context.Users.FindAsync(userId);

                if (user == null)
                    throw new Exception("User not found");

                //if (user.LastDailyChallengeDate?.Date == DateTime.UtcNow.Date)
                //    return new Response("", "", "", "✅ You already completed today’s challenge.");

                Console.WriteLine($"Language = {user.TechnoStack}, Experience = {user.Experience}");

                var snippet = await _snippets.Find(s =>
                    s.Language == user.TechnoStack &&
                    s.Experience == user.Experience).FirstOrDefaultAsync(cancellationToken);

                if (snippet is null)
                    throw new Exception("Snippet not found");

                bool correctIsA = new Random().Next(2) == 0;
                string a = correctIsA ? snippet.CorrectCode : snippet.BuggyCode;
                string b = correctIsA ? snippet.BuggyCode : snippet.CorrectCode;

                cache.Set($"casino:daily:{userId}:correct", correctIsA ? "a" : "b", TimeSpan.FromHours(24));
              //  user.LastDailyChallengeDate = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return new Response(a, b, snippet.Explanation, "🎯 Your daily challenge is ready!");
            }
        }
    }
}
