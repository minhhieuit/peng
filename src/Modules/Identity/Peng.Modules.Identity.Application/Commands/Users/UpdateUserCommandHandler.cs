using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Users;

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand, UserDto>
{
    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null) return Result.Failure<UserDto>(Error.NotFound("User"));

        user.UpdateProfile(request.FirstName, request.LastName);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var permissions = user.GetAllPermissions().ToList();
        return Result.Success(new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.FullName,
            user.IsActive, user.CreatedAt,
            user.UserRoles.Select(ur => ur.Role?.Name ?? "").Where(r => !string.IsNullOrEmpty(r)),
            permissions));
    }
}
