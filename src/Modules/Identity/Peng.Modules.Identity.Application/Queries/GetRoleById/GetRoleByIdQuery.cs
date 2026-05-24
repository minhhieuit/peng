using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IQuery<RoleDto>;
