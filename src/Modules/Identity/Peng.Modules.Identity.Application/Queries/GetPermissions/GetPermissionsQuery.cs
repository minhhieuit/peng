using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Queries.GetPermissions;

public record GetPermissionsQuery : IQuery<IReadOnlyList<PermissionDto>>;
