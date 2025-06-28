using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DevLife_Portal.Features.EscapeMeeting
{
    public class SaveFavoriteExcuse
    {
        public record Request(string Category, string Excuse);

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Category).NotEmpty();
                RuleFor(x => x.Excuse).NotEmpty();
            }
        }

        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("/escape/favorite", Handler).WithTags("EscapeMeeting");
            }
        }

        public static async Task<IResult> Handler(
         [FromBody] Request request,
         IValidator<Request> validator,
         HttpContext context,
         RedisService redis,
         CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            var userId = context.Session.GetString("userId");
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Results.Unauthorized();
            }

            var key = $"favorites:{userId}";
            redis.AddToQueue(key, request); 

            return Results.Ok(new { Message = "Saved to favorites." });
        }

    }
}
