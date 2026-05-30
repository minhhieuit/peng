using MediatR;
using Microsoft.AspNetCore.Mvc;
using Peng.Modules.Identity.Application.Commands.Roles;
using Peng.Modules.Identity.Application.Queries.GetRoleById;
using Peng.Modules.Identity.Application.Queries.GetRoles;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints.Identity;

internal class RoleEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/roles").WithTags("Roles").RequireAuthorization("AdminToken");

        group.MapGet("/", GetRoles)
             .WithSummary("Get all roles")
             .RequireAuthorization(p => p.RequireClaim("permission", "identity:roles:read"));

        group.MapGet("/{id:guid}", GetRoleById)
             .WithSummary("Get role by ID")
             .RequireAuthorization(p => p.RequireClaim("permission", "identity:roles:read"));

        group.MapPost("/", CreateRole)
             .WithSummary("Create a new role")
             .RequireAuthorization(p => p.RequireClaim("permission", "identity:roles:write"));

        group.MapPut("/{id:guid}", UpdateRole)
             .WithSummary("Update a role")
             .RequireAuthorization(p => p.RequireClaim("permission", "identity:roles:write"));

        group.MapDelete("/{id:guid}", DeleteRole)
             .WithSummary("Delete a role (non-system only)")
             .RequireAuthorization(p => p.RequireClaim("permission", "identity:roles:write"));

        group.MapPut("/{id:guid}/permissions", AssignPermissions)
             .WithSummary("Set permissions for a role")
             .RequireAuthorization(p => p.RequireClaim("permission", "identity:roles:write"));
    }

    private static async Task<IResult> GetRoles(ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new GetRolesQuery(), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest();
    }

    private static async Task<IResult> GetRoleById(Guid id, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new GetRoleByIdQuery(id), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
    }

    private static async Task<IResult> CreateRole([FromBody] CreateRoleCommand command, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(command, ct);
        if (result.IsSuccess) return Results.Created($"/api/roles/{result.Value.Id}", result.Value);
        return result.Error.Code.StartsWith("Validation")
            ? Results.UnprocessableEntity(new { result.Error.Code, result.Error.Description })
            : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }

    private static async Task<IResult> UpdateRole(Guid id, [FromBody] UpdateRoleRequest body, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new UpdateRoleCommand(id, body.Name, body.Description), ct);
        if (result.IsSuccess) return Results.Ok(result.Value);
        return result.Error.Code.StartsWith("Validation")
            ? Results.UnprocessableEntity(new { result.Error.Code, result.Error.Description })
            : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }

    private static async Task<IResult> DeleteRole(Guid id, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new DeleteRoleCommand(id), ct);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }

    private static async Task<IResult> AssignPermissions(Guid id, [FromBody] AssignPermissionsRequest body, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new AssignPermissionsToRoleCommand(id, body.PermissionCodes), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }
}

internal record UpdateRoleRequest(string Name, string Description);
internal record AssignPermissionsRequest(IEnumerable<string> PermissionCodes);
