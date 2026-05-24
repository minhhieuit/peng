using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Roles;

public record CreateRoleCommand(string Name, string Description) : ICommand<RoleDto>;
