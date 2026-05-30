using Peng.Modules.Members.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.DeactivateMember;

internal sealed class DeactivateMemberCommandHandler(
    IMemberRepository memberRepository,
    IMembersUnitOfWork unitOfWork) : ICommandHandler<DeactivateMemberCommand>
{
    public async Task<Result> Handle(DeactivateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken);
        if (member is null) return Result.Failure(Error.NotFound("Member"));

        member.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
