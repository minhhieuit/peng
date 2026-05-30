using MediatR;
using Microsoft.AspNetCore.Mvc;
using Peng.Modules.Members.Application.Commands.MemberLogin;
using Peng.Modules.Members.Application.Commands.RegisterMember;
using Peng.Modules.Members.Application.Queries.GetMemberById;
using Peng.SharedKernel.Application;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints.Members;

internal class MemberAuthEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/api/auth/member").WithTags("Member Auth");

        auth.MapPost("/register", Register)
            .WithSummary("Register a new member account (public client)")
            .AllowAnonymous();

        auth.MapPost("/login", Login)
            .WithSummary("Login as a member and receive a member-scoped JWT")
            .AllowAnonymous();

        app.MapGet("/api/members/me", GetCurrentMember)
           .WithTags("Members")
           .WithSummary("Get the currently authenticated member")
           .RequireAuthorization("MemberToken");
    }

    private static async Task<IResult> Register([FromBody] RegisterMemberCommand command, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(command, ct);
        if (result.IsSuccess) return Results.Ok(result.Value);
        return result.Error.Code.StartsWith("Validation")
            ? Results.UnprocessableEntity(new { result.Error.Code, result.Error.Description })
            : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }

    private static async Task<IResult> Login([FromBody] MemberLoginCommand command, ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(command, ct);
        if (result.IsSuccess) return Results.Ok(result.Value);
        return result.Error.Code.StartsWith("Validation")
            ? Results.UnprocessableEntity(new { result.Error.Code, result.Error.Description })
            : Results.Unauthorized();
    }

    private static async Task<IResult> GetCurrentMember(ICurrentUser currentUser, ISender sender, CancellationToken ct)
    {
        if (currentUser.UserId is null) return Results.Unauthorized();
        var result = await sender.Send(new GetMemberByIdQuery(currentUser.UserId.Value), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
    }
}
