using MediatR;
using Microsoft.AspNetCore.Mvc;
using Peng.Modules.Identity.Application.Commands.Login;
using Peng.Modules.Identity.Application.Commands.Register;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints.Identity;

internal class AuthEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", Register)
             .WithName("Register")
             .WithSummary("Register a new user account")
             .AllowAnonymous();

        group.MapPost("/login", Login)
             .WithName("Login")
             .WithSummary("Login and receive a JWT token")
             .AllowAnonymous();
    }

    private static async Task<IResult> Register(
        [FromBody] RegisterCommand command,
        ISender sender,
        CancellationToken ct)
    {
        var result = await sender.Send(command, ct);
        if (result.IsSuccess) return Results.Ok(result.Value);
        return result.Error.Code.StartsWith("Validation")
            ? Results.UnprocessableEntity(new { result.Error.Code, result.Error.Description })
            : Results.BadRequest(new { result.Error.Code, result.Error.Description });
    }

    private static async Task<IResult> Login(
        [FromBody] LoginCommand command,
        ISender sender,
        CancellationToken ct)
    {
        var result = await sender.Send(command, ct);
        if (result.IsSuccess) return Results.Ok(result.Value);
        return result.Error.Code.StartsWith("Validation")
            ? Results.UnprocessableEntity(new { result.Error.Code, result.Error.Description })
            : Results.Unauthorized();
    }
}
