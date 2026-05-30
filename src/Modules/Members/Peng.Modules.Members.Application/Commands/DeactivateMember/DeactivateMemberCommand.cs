using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.DeactivateMember;

public record DeactivateMemberCommand(Guid MemberId) : ICommand;
