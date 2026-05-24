using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Queries.GetRoles;

public record GetRolesQuery : IQuery<IReadOnlyList<RoleDto>>;
