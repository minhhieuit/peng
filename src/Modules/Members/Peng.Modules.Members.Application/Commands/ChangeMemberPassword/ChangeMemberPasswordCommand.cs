using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.ChangeMemberPassword;

/// <summary>Admin sets a new password for a member.</summary>
public record ChangeMemberPasswordCommand(Guid MemberId, string NewPassword) : ICommand;
