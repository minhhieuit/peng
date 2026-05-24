using Peng.Modules.Identity.Application.Commands.Roles;
using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Queries.GetRoles;

internal sealed class GetRolesQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetRolesQuery, IReadOnlyList<RoleDto>>
{
    public async Task<Result<IReadOnlyList<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllAsync(cancellationToken);
        var dtos = roles.Select(CreateRoleCommandHandler.MapToDto).ToList();
        return Result.Success<IReadOnlyList<RoleDto>>(dtos);
    }
}
