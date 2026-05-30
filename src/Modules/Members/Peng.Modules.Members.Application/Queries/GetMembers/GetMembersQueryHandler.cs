using Peng.Modules.Members.Application.DTOs;
using Peng.Modules.Members.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Queries.GetMembers;

internal sealed class GetMembersQueryHandler(IMemberRepository memberRepository)
    : IQueryHandler<GetMembersQuery, PagedList<MemberDto>>
{
    public async Task<Result<PagedList<MemberDto>>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        var (members, total) = await memberRepository.GetPagedAsync(request.Page, request.PageSize, request.Search, cancellationToken);
        var dtos = members.Select(m => m.ToDto()).ToList();
        return Result.Success(new PagedList<MemberDto>(dtos, request.Page, request.PageSize, total));
    }
}
