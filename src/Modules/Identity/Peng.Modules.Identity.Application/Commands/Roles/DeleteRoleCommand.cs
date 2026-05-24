using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Roles;

public record DeleteRoleCommand(Guid Id) : ICommand;
