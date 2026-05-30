using Peng.Modules.Members.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.ChangeMemberPassword;

internal sealed class ChangeMemberPasswordCommandHandler(
    IMemberRepository memberRepository,
    IPasswordHasher passwordHasher,
    IMembersUnitOfWork unitOfWork) : ICommandHandler<ChangeMemberPasswordCommand>
{
    public async Task<Result> Handle(ChangeMemberPasswordCommand request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken);
        if (member is null) return Result.Failure(Error.NotFound("Member"));

        var passwordHash = passwordHasher.Hash(request.NewPassword);
        member.ChangePassword(passwordHash);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
