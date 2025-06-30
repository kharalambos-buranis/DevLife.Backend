using DevLife_Portal.Common.Enums;
using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Models;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevLife_Portal.Features.Auth
{
    public class LoginUser
    {
        public record Request(string Username);

        public record Response(string Username, string FullName, string Technostack, Experience Experience, ZodiacSign Sign, int Points, int Streak);

        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(u => u.Username).NotEmpty();
            }
        }

        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("api/users/login", Handler).WithTags("Users");
            }
        }

        public static async Task<IResult> Handler(
            [FromBody] Request request,
            AppDbContext context,
            IValidator<Request> validator,
            // TokenProvider token,
            HttpContext httpContext,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var user = await context.Users
              .Include(u => u.ZodiacSign)
              .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user is null)
            {
                return Results.Unauthorized();
            }

            httpContext.Session.SetString("userId", user.Id.ToString());

            return Results.Ok(new
            {
                User = new
                {
                    user.Id,
                    user.Username,
                    user.Name,
                    user.Lastname,
                    user.ZodiacSign
                }
            });

        }
    }
} 
