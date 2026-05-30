using Peng.Modules.Members.Application.DTOs;
using Peng.Modules.Members.Domain.Entities;
using Peng.Modules.Members.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.RegisterMember;

internal sealed class RegisterMemberCommandHandler(
    IMemberRepository memberRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IMembersUnitOfWork unitOfWork) : ICommandHandler<RegisterMemberCommand, MemberAuthResponse>
{
    public async Task<Result<MemberAuthResponse>> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
    {
        if (await memberRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            return Result.Failure<MemberAuthResponse>(Error.Conflict("Member"));

        var passwordHash = passwordHasher.Hash(request.Password);
        var member = Member.Register(request.Email, request.FirstName, request.LastName, passwordHash);

        await memberRepository.AddAsync(member, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var token = jwtService.GenerateToken(member.Id, member.Email, member.FirstName, member.LastName,
            ["Member"], [], TokenTypes.Member);

        return Result.Success(new MemberAuthResponse(token, "Bearer", 3600, member.ToDto()));
    }
}
