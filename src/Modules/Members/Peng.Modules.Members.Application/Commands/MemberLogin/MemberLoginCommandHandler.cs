using Peng.Modules.Members.Application.DTOs;
using Peng.Modules.Members.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.MemberLogin;

internal sealed class MemberLoginCommandHandler(
    IMemberRepository memberRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IMembersUnitOfWork unitOfWork) : ICommandHandler<MemberLoginCommand, MemberAuthResponse>
{
    private static readonly Error InvalidCredentials = new("Auth.InvalidCredentials", "Invalid email or password.");

    public async Task<Result<MemberAuthResponse>> Handle(MemberLoginCommand request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (member is null) return Result.Failure<MemberAuthResponse>(InvalidCredentials);
        if (!member.IsActive) return Result.Failure<MemberAuthResponse>(new Error("Auth.AccountDisabled", "Account is disabled."));
        if (!passwordHasher.Verify(request.Password, member.PasswordHash))
            return Result.Failure<MemberAuthResponse>(InvalidCredentials);

        member.RecordLogin();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var token = jwtService.GenerateToken(member.Id, member.Email, member.FirstName, member.LastName,
            ["Member"], [], TokenTypes.Member);

        return Result.Success(new MemberAuthResponse(token, "Bearer", 3600, member.ToDto()));
    }
}
