using Peng.Modules.Identity.Application.Commands.Roles;
using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Queries.GetRoleById;

internal sealed class GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetRoleByIdQuery, RoleDto>
{
    public async Task<Result<RoleDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.Id, cancellationToken);
        return role is null
            ? Result.Failure<RoleDto>(Error.NotFound("Role"))
            : Result.Success(CreateRoleCommandHandler.MapToDto(role));
    }
}
