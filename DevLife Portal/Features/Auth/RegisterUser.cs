using DevLife_Portal.Common.Enums;
using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Models;
using DevLife_Portal.Common.Services;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevLife_Portal.Features.Auth
{
    public class RegisterUser
    {
        public record Request(string Username, string Name, string LastName, DateTime DateOfBirth, string TechnoStack, Experience Experience);
        public record Response(int Id, string UserName, string Name, string Lastname, ZodiacSign Sign);

        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(u => u.Username)
           .NotEmpty();

                RuleFor(u => u.Name)
                    .NotEmpty()
                    .Matches("^[a-zA]+$")
                    .WithMessage("Name must contain only letters (no numbers or symbols).");

                RuleFor(u => u.LastName)
                    .NotEmpty()
                    .Matches("^[a-zA]+$")
                    .WithMessage("LastName must contain only letters (no numbers or symbols).");

                RuleFor(u => u.DateOfBirth)
                    .NotEmpty()
                    .Must(BeAValidDate)
                    .WithMessage("Date of birth must be a valid date (no letters).");

                RuleFor(u => u.TechnoStack)
                    .NotEmpty();

                RuleFor(u => u.Experience)
                    .NotEmpty();

            }

            private bool BeAValidDate(DateTime date)
            {
                return date < DateTime.UtcNow && date.Year > 1900;
            }

            public sealed class Endpoint : IEndpoint
            {
                public void MapEndpoint(IEndpointRouteBuilder app)
                {
                    app.MapPost("api/users", Handler).WithTags("Users");
                }
            }

            public static async Task<IResult> Handler([FromBody] Request request, IValidator<Request> validator, CancellationToken cancellationToken, AppDbContext context, ZodiacService zodiacService)
            {
                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var existingUser = await context.Users
                 .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

                if (existingUser is not null)
                {
                    //logger.LogWarning("Attempt to register with already used UserName {UserName}", request.Username);
                    return Results.BadRequest("UserName is already registered.");
                }

                var zodiacSign = await zodiacService.GetZodiacSignAsync(request.DateOfBirth, cancellationToken);
                if (zodiacSign is null)
                {
                    return Results.BadRequest("Unable to determine zodiac sign from date of birth.");
                }

                var user = new User
                {
                    Username = request.Username,
                    Name = request.Name,
                    Lastname = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    TechnoStack = request.TechnoStack,
                    Experience = request.Experience,
                    ZodiacSign = zodiacSign
                   
                };

                context.Users.Add(user);

                await context.SaveChangesAsync();

                return Results.Ok(new Response(user.Id,user.Username,user.Name,user.Lastname,user.ZodiacSign));
            }
        }
    }
}
