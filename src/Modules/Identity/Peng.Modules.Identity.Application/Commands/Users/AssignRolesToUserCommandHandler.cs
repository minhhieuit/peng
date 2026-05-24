using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Users;

internal sealed class AssignRolesToUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AssignRolesToUserCommand, UserDto>
{
    public async Task<Result<UserDto>> Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null) return Result.Failure<UserDto>(Error.NotFound("User"));

        // Remove all existing roles, then assign selected
        foreach (var ur in user.UserRoles.ToList())
            user.RemoveRole(ur.RoleId);

        var roleIds = request.RoleIds.ToList();
        foreach (var roleId in roleIds)
        {
            var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
            if (role is not null) user.AssignRole(role);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var permissions = user.GetAllPermissions().ToList();
        return Result.Success(new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.FullName,
            user.IsActive, user.CreatedAt,
            user.UserRoles.Select(ur => ur.Role?.Name ?? "").Where(r => !string.IsNullOrEmpty(r)),
            permissions));
    }
}
