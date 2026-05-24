using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Roles;

public record UpdateRoleCommand(Guid Id, string Name, string Description) : ICommand<RoleDto>;
