using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Queries.GetPermissions;

internal sealed class GetPermissionsQueryHandler(IPermissionRepository permissionRepository)
    : IQueryHandler<GetPermissionsQuery, IReadOnlyList<PermissionDto>>
{
    public async Task<Result<IReadOnlyList<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var perms = await permissionRepository.GetAllAsync(cancellationToken);
        var dtos = perms.Select(p => new PermissionDto(p.Id, p.Code, p.Name, p.Description, p.Module)).ToList();
        return Result.Success<IReadOnlyList<PermissionDto>>(dtos);
    }
}
