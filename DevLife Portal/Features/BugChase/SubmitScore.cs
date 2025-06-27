using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Models;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevLife_Portal.Features.BugChase
{
    public class SubmitScore
    {
        public record Request(int Score);
        public record Response(string Message);

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Score).GreaterThanOrEqualTo(0);
            }
        }

        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("/api/bugchase/score", Handler)
                   .WithTags("BugChase");
            }
        }

        public static async Task<IResult> Handler(
            [FromBody] Request request,
            IValidator<Request> validator,
            AppDbContext context,
            HttpContext httpContext,
            CancellationToken cancelationToken)
        {
            var validation = await validator.ValidateAsync(request, cancelationToken);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            var userIdStr = httpContext.Session.GetString("userId");
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancelationToken);
            if (user == null)
                return Results.NotFound("User not found");

            var score = new BugChaseScore
            {
                UserId = user.Id,
                Score = request.Score,
                Timestamp = DateTime.UtcNow
            };

            context.BugChaseScores.Add(score);
            await context.SaveChangesAsync(cancelationToken);

            return Results.Ok(new Response("Score submitted successfully!"));
        }
    }
}
