using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Services;

namespace DevLife_Portal.Features.EscapeMeeting
{
    public class GetExcuseCategories
    {
        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("/escape/categories", () =>
                    Results.Ok(ExcuseCategories.Categories)
                ).WithTags("EscapeMeeting");
            }
        }
    }
}
