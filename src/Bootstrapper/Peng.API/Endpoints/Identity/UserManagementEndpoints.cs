using MediatR;
using Microsoft.AspNetCore.Mvc;
using Peng.Modules.Identity.Application.Commands.Users;
using Peng.Modules.Identity.Application.Queries.GetUserById;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints.Identity;

internal class UserManagementEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users").RequireAuthorization();

        group.MapGet("/{id:guid}", GetUserById)
             .WithSummary("Get user by ID")
             .RequireAuthorization(p => p.RequireClaim("permission", "identity:users:read"));

        group.MapPut("/{id:guid}", UpdateUser)
             .WithSummary("Update user profile")
             .RequireAuthorization(p => p.RequireClaim("permission", "identity:users:write"));

        group.MapPut("/{id:guid}/roles", AssignRoles)
             .WithSummary("Set roles for a user")
             .RequireAuthorization(p => p.RequireClaim("permission", "identity:users:write"));

        group.MapPatch("/{id:guid}/deactivate", DeactivateUser)
             .WithSummary("Deactivate a user")
             .RequireAuthorization(p => p.RequireClaim("permission", "identity:users:delete"));
    }

    private static async Task<IResult> GetUserById(Guid id, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new GetUserByIdQuery(id), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
    }

    private static async Task<IResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest body, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new UpdateUserCommand(id, body.FirstName, body.LastName), ct);
        if (result.IsSuccess) return Results.Ok(result.Value);
        return result.Error.Code.StartsWith("Validation")
            ? Results.UnprocessableEntity(new { result.Error.Code, result.Error.Description })
            : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }

    private static async Task<IResult> AssignRoles(Guid id, [FromBody] AssignRolesRequest body, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new AssignRolesToUserCommand(id, body.RoleIds), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }

    private static async Task<IResult> DeactivateUser(Guid id, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new DeactivateUserCommand(id), ct);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }
}

internal record UpdateUserRequest(string FirstName, string LastName);
internal record AssignRolesRequest(IEnumerable<Guid> RoleIds);
