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

        public record Response(string Username, string FullName, string Technostack, string Experience, ZodiacSign Sign, int Points, int Streak);

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
                //logger.LogWarning("Invalid login input for {Email}", request.Email);
                return Results.BadRequest(validationResult.Errors);
            }

            var user = await context.Users
              .Include(u => u.ZodiacSign)
              .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user is null)
            {
                // logger.LogWarning("Login failed. Email not found: {Email}", request.Email);
                return Results.Unauthorized();
            }

            // var accessToken = token.Create(user);
            httpContext.Session.SetString("userId", user.Id.ToString());

            return Results.Ok(new
            {
                // Token = accessToken,
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
