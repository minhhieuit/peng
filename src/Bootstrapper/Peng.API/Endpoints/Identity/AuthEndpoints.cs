using MediatR;
using Microsoft.AspNetCore.Mvc;
using Peng.Modules.Identity.Application.Commands.Login;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints.Identity;

internal class AuthEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        // Admin/staff login only. Members authenticate via /api/auth/member/*.
        // There is no public self-registration for admin users — they are created
        // by the seeder or (in the future) by another admin.
        group.MapPost("/login", Login)
             .WithName("Login")
             .WithSummary("Login as an admin user and receive a JWT token")
             .AllowAnonymous();
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
