using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Users;

public record AssignRolesToUserCommand(Guid UserId, IEnumerable<Guid> RoleIds) : ICommand<UserDto>;
