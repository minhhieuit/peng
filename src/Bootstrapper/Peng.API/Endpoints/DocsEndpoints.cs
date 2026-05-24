using Peng.Modules.Identity.Application;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints;

internal class DocsEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/docs").WithTags("Documentation");

        group.MapGet("/modules", GetModules)
             .WithName("GetModules")
             .WithSummary("Get all registered modules with their features and permissions")
             .AllowAnonymous();
    }

    private static IResult GetModules()
    {
        var descriptors = new List<IModuleDescriptor>
        {
            new IdentityModuleDescriptor(),
        };

        var result = descriptors.Select(d => new
        {
            d.ModuleName,
            d.Description,
            d.Version,
            Permissions = d.Permissions,
            Features = d.Features,
        });

        return Results.Ok(result);
    }
}
