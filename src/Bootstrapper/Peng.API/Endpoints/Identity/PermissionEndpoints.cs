using MediatR;
using Peng.Modules.Identity.Application.Queries.GetPermissions;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints.Identity;

internal class PermissionEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/permissions").WithTags("Permissions")
           .RequireAuthorization("AdminToken")
           .MapGet("/", GetPermissions)
           .WithSummary("Get all permissions")
           .RequireAuthorization(p => p.RequireClaim("permission", "identity:permissions:read"));
    }

    private static async Task<IResult> GetPermissions(ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new GetPermissionsQuery(), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest();
    }
}
