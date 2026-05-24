using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Roles;

public record AssignPermissionsToRoleCommand(Guid RoleId, IEnumerable<string> PermissionCodes) : ICommand<RoleDto>;
