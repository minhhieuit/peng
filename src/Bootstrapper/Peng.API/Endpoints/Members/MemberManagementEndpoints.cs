using MediatR;
using Microsoft.AspNetCore.Mvc;
using Peng.Modules.Members.Application.Commands.ChangeMemberPassword;
using Peng.Modules.Members.Application.Commands.CreateMember;
using Peng.Modules.Members.Application.Commands.DeactivateMember;
using Peng.Modules.Members.Application.Commands.UpdateMember;
using Peng.Modules.Members.Application.Queries.GetMemberById;
using Peng.Modules.Members.Application.Queries.GetMembers;
using Peng.SharedKernel.Application;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints.Members;

internal class MemberManagementEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        // Admin-only management of members. Requires an admin token (audience guard)
        // on top of the per-endpoint permission claims.
        var group = app.MapGroup("/api/members").WithTags("Members")
            .RequireAuthorization("AdminToken");

        group.MapGet("/", GetMembers)
             .WithSummary("Get paginated list of members")
             .RequireAuthorization(p => p.RequireClaim("permission", "members:members:read"));

        group.MapGet("/{id:guid}", GetMemberById)
             .WithSummary("Get member by ID")
             .RequireAuthorization(p => p.RequireClaim("permission", "members:members:read"));

        group.MapPost("/", CreateMember)
             .WithSummary("Create a member account manually (returns a temporary password)")
             .RequireAuthorization(p => p.RequireClaim("permission", "members:members:write"));

        group.MapPut("/{id:guid}", UpdateMember)
             .WithSummary("Update a member's profile")
             .RequireAuthorization(p => p.RequireClaim("permission", "members:members:write"));

        group.MapPatch("/{id:guid}/password", ChangePassword)
             .WithSummary("Set a new password for a member")
             .RequireAuthorization(p => p.RequireClaim("permission", "members:members:write"));

        group.MapPatch("/{id:guid}/deactivate", DeactivateMember)
             .WithSummary("Deactivate a member")
             .RequireAuthorization(p => p.RequireClaim("permission", "members:members:write"));
    }

    private static async Task<IResult> GetMembers(
        ISender sender,
        int page = 1, int pageSize = 20, string? search = null,
        CancellationToken ct = default)
    {
        var result = await sender.Send(new GetMembersQuery(page, pageSize, search), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest();
    }

    private static async Task<IResult> GetMemberById(Guid id, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new GetMemberByIdQuery(id), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
    }

    private static async Task<IResult> CreateMember(
        [FromBody] CreateMemberRequest body, ICurrentUser currentUser, ISender sender, CancellationToken ct)
    {
        if (currentUser.UserId is null) return Results.Unauthorized();
        var result = await sender.Send(
            new CreateMemberCommand(body.Email, body.FirstName, body.LastName, currentUser.UserId.Value), ct);
        if (result.IsSuccess) return Results.Ok(result.Value);
        return result.Error.Code.StartsWith("Validation")
            ? Results.UnprocessableEntity(new { result.Error.Code, result.Error.Description })
            : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }

    private static async Task<IResult> UpdateMember(Guid id, [FromBody] UpdateMemberRequest body, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new UpdateMemberCommand(id, body.FirstName, body.LastName), ct);
        if (result.IsSuccess) return Results.Ok(result.Value);
        return result.Error.Code.StartsWith("Validation")
            ? Results.UnprocessableEntity(new { result.Error.Code, result.Error.Description })
            : result.Error.Code.Contains("NotFound") ? Results.NotFound()
            : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }

    private static async Task<IResult> ChangePassword(Guid id, [FromBody] ChangeMemberPasswordRequest body, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new ChangeMemberPasswordCommand(id, body.NewPassword), ct);
        if (result.IsSuccess) return Results.NoContent();
        return result.Error.Code.StartsWith("Validation")
            ? Results.UnprocessableEntity(new { result.Error.Code, result.Error.Description })
            : result.Error.Code.Contains("NotFound") ? Results.NotFound()
            : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }

    private static async Task<IResult> DeactivateMember(Guid id, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new DeactivateMemberCommand(id), ct);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }
}

internal record CreateMemberRequest(string Email, string FirstName, string LastName);
internal record UpdateMemberRequest(string FirstName, string LastName);
internal record ChangeMemberPasswordRequest(string NewPassword);
