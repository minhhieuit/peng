using Peng.Modules.Members.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.UpdateMember;

public record UpdateMemberCommand(Guid MemberId, string FirstName, string LastName) : ICommand<MemberDto>;
