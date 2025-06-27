using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DevLife_Portal.Features.Roast
{
    public class RoastCode
    {
        public record Request(string Code, string Language, string ChallengeTitle);
        public record Response(string Verdict, string RoastMessage);

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Code).NotEmpty();
                RuleFor(x => x.Language).NotEmpty();
                RuleFor(x => x.ChallengeTitle).NotEmpty();
            }
        }

        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("/api/roast", Handler)
                   .WithTags("Roast");
            }
        }

        private static string Normalize(string code)
        {
            return code
                .Replace(" ", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("\t", "")
                .ToLowerInvariant();
        }

        public static async Task<IResult> Handler(
           [FromBody] Request request,
           IValidator<Request> validator,
           ICodeExecutionService codeExecutionService,
           IAiContentService aiContentService,
           HttpContext httpContext,
           CancellationToken cancellationToken)
            {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            // Check if we have a correct answer
            if (!CodeWarsChallengeService.CorrectAnswers.TryGetValue(request.ChallengeTitle, out var correctAnswer))
            {
                return Results.NotFound("Challenge title not recognized.");
            }

            // Simple normalization (ignore whitespace, etc.)
            bool isCorrect = Normalize(request.Code) == Normalize(correctAnswer);

            var verdict = isCorrect ? "Success" : "Fail";
            var roast = await aiContentService.GenerateRoastAsync(request.Code, verdict, cancellationToken);

             return Results.Ok(new Response(verdict, roast));
          }

        //public static async Task<IResult> Handler(
        //    [FromBody] Request request,
        //    IValidator<Request> validator,
        //    ICodeExecutionService codeExecutionService,
        //    IAiContentService aiContentService,
        //    HttpContext httpContext,
        //    CancellationToken cancellationToken)
        //{
        //    var validation = await validator.ValidateAsync(request, cancellationToken);
        //    if (!validation.IsValid)
        //        return Results.BadRequest(validation.Errors);

        //    var executionResult = await codeExecutionService.ExecuteCodeAsync(request.Code, request.Language, cancellationToken);

        //    var roast = await aiContentService.GenerateRoastAsync(request.Code, executionResult, cancellationToken);

        //    return Results.Ok(new Response(executionResult, roast));
        //}
    }
}
