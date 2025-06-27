using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Models;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DevLife_Portal.Features.Casino
{
    public static class GetSnippetsFromCodewars
    {
        public record Request() : IRequest<Response>;

        public record Response(string A, string B, string Explanation);

        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("api/casino/snippet", Handler)
                    .WithTags("Casino");
            }

            public static async Task<IResult> Handler(
                HttpContext httpContext,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken)
            {
                var response = await mediator.Send(new Request(), cancellationToken);
                return Results.Ok(response);
            }
        }


        public sealed class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMongoCollection<CodeSnippet> _snippets;
            private readonly AppDbContext _db;
            private readonly IHttpContextAccessor _httpContext;
            private readonly IMemoryCache _cache;

            public Handler(AppDbContext db, IMongoDatabase mongo, IHttpContextAccessor context, IMemoryCache cache)
            {
                _snippets = mongo.GetCollection<CodeSnippet>("casino_snippets");
                _db = db;
                _httpContext = context;
                _cache = cache;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                //var userId = int.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var userIdString = _httpContext.HttpContext?.Session.GetString("userId");

                if (string.IsNullOrWhiteSpace(userIdString))
                {
                    throw new Exception("User is not logged in or session expired.");
                }

                var userId = int.Parse(userIdString);
                var user = await _db.Users.FindAsync(userId);

                var filter = Builders<CodeSnippet>.Filter.Regex("Language", new BsonRegularExpression(user.TechnoStack, "i")) &
                Builders<CodeSnippet>.Filter.Regex("Experience", new BsonRegularExpression(user.Experience, "i"));

                Console.WriteLine($"User Stack: {user.TechnoStack}, Level: {user.Experience}");

                var snippet = await _snippets.Find(filter).FirstOrDefaultAsync(); 

                if (snippet is null)
                {
                    throw new Exception("No snippet found");
                }

                var random = new Random();
                bool correctIsA = random.Next(2) == 0;

                var a = correctIsA ? snippet.CorrectCode : snippet.BuggyCode;
                var b = correctIsA ? snippet.BuggyCode : snippet.CorrectCode;

                _cache.Set($"casino:{userId}:correct", correctIsA ? "a" : "b", TimeSpan.FromMinutes(5));

                return new Response(a, b, snippet.Explanation);
            }
        }
    }
}
