using Peng.Modules.Members.Application.DTOs;
using Peng.Modules.Members.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.UpdateMember;

internal sealed class UpdateMemberCommandHandler(
    IMemberRepository memberRepository,
    IMembersUnitOfWork unitOfWork) : ICommandHandler<UpdateMemberCommand, MemberDto>
{
    public async Task<Result<MemberDto>> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken);
        if (member is null) return Result.Failure<MemberDto>(Error.NotFound("Member"));

        member.UpdateProfile(request.FirstName, request.LastName);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(member.ToDto());
    }
}
