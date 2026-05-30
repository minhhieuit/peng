using MediatR;
using Peng.Modules.Identity.Application.Queries.GetCurrentUser;
using Peng.Modules.Identity.Application.Queries.GetUsers;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints.Identity;

internal class UserEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users").RequireAuthorization("AdminToken");

        group.MapGet("/me", GetCurrentUser)
             .WithName("GetCurrentUser")
             .WithSummary("Get the currently authenticated user");

        group.MapGet("/", GetUsers)
             .WithName("GetUsers")
             .WithSummary("Get paginated list of users")
             .RequireAuthorization(policy => policy.RequireClaim("permission", "identity:users:read"));
    }

    private static async Task<IResult> GetCurrentUser(ISender sender, CancellationToken ct)
    {
        var result = await sender.Send(new GetCurrentUserQuery(), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.Unauthorized();
    }

    private static async Task<IResult> GetUsers(
        ISender sender,
        int page = 1, int pageSize = 20, string? search = null,
        CancellationToken ct = default)
    {
        var result = await sender.Send(new GetUsersQuery(page, pageSize, search), ct);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest();
    }
}
