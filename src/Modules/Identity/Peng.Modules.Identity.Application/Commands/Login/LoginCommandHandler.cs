using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Login;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IUnitOfWork unitOfWork) : ICommandHandler<LoginCommand, AuthResponse>
{
    private static readonly Error InvalidCredentials = new("Auth.InvalidCredentials", "Invalid email or password.");

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null) return Result.Failure<AuthResponse>(InvalidCredentials);
        if (!user.IsActive) return Result.Failure<AuthResponse>(new Error("Auth.AccountDisabled", "Account is disabled."));
        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Failure<AuthResponse>(InvalidCredentials);

        user.RecordLogin();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var permissions = user.GetAllPermissions().ToList();
        var roles = user.UserRoles.Select(ur => ur.Role?.Name ?? "").Where(r => !string.IsNullOrEmpty(r)).ToList();
        var token = jwtService.GenerateToken(user.Id, user.Email, user.FirstName, user.LastName, roles, permissions, TokenTypes.Admin);

        var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.FullName,
            user.IsActive, user.CreatedAt, roles, permissions);

        return Result.Success(new AuthResponse(token, "Bearer", 3600, userDto));
    }
}
