using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Application.Services;
using Peng.Modules.Identity.Domain.Entities;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Register;

internal sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IUnitOfWork unitOfWork) : ICommandHandler<RegisterCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            return Result.Failure<AuthResponse>(Error.Conflict("User"));

        var passwordHash = passwordHasher.Hash(request.Password);
        var user = User.Create(request.Email, request.FirstName, request.LastName, passwordHash);

        var memberRole = await roleRepository.GetByNameAsync("Member", cancellationToken);
        if (memberRole is not null) user.AssignRole(memberRole);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var permissions = user.GetAllPermissions().ToList();
        var token = jwtService.GenerateToken(user, permissions);

        var userDto = MapToDto(user, permissions);
        return Result.Success(new AuthResponse(token, "Bearer", 3600, userDto));
    }

    private static UserDto MapToDto(User user, IEnumerable<string> permissions) =>
        new(user.Id, user.Email, user.FirstName, user.LastName, user.FullName,
            user.IsActive, user.CreatedAt,
            user.UserRoles.Select(ur => ur.Role?.Name ?? "").Where(r => !string.IsNullOrEmpty(r)),
            permissions);
}
