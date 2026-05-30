using Peng.Modules.Members.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Queries.GetMembers;

public record GetMembersQuery(int Page = 1, int PageSize = 20, string? Search = null) : IQuery<PagedList<MemberDto>>;
