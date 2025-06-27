using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevLife_Portal.Features.EscapeMeeting
{
    public class GetRandomExcuse
    {
        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("/escape/excuse", Handler).WithTags("EscapeMeeting");
            }

            public static IResult Handler([FromQuery] string category)
            {
                if (string.IsNullOrWhiteSpace(category) || !ExcuseBank.Excuses.TryGetValue(category, out var excuses))
                {
                    return Results.BadRequest("Invalid or missing category.");
                }

                var random = new Random();
                var excuse = excuses[random.Next(excuses.Count)];
                return Results.Ok(new { Category = category, Excuse = excuse });
            }
        }
    }
}
