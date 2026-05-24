using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null) return Result.Failure<UserDto>(Error.NotFound("User"));

        var permissions = user.GetAllPermissions().ToList();
        return Result.Success(new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.FullName,
            user.IsActive, user.CreatedAt,
            user.UserRoles.Select(ur => ur.Role?.Name ?? "").Where(r => !string.IsNullOrEmpty(r)),
            permissions));
    }
}
