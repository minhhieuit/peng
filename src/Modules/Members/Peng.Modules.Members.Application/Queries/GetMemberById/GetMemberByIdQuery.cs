using Peng.Modules.Members.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Queries.GetMemberById;

public record GetMemberByIdQuery(Guid Id) : IQuery<MemberDto>;
