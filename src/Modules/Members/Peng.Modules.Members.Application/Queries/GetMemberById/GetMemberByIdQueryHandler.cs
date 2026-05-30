using Peng.Modules.Members.Application.DTOs;
using Peng.Modules.Members.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Queries.GetMemberById;

internal sealed class GetMemberByIdQueryHandler(IMemberRepository memberRepository)
    : IQueryHandler<GetMemberByIdQuery, MemberDto>
{
    public async Task<Result<MemberDto>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByIdAsync(request.Id, cancellationToken);
        return member is null
            ? Result.Failure<MemberDto>(Error.NotFound("Member"))
            : Result.Success(member.ToDto());
    }
}
