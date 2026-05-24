using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Entities;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Roles;

internal sealed class CreateRoleCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateRoleCommand, RoleDto>
{
    public async Task<Result<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var existing = await roleRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existing is not null)
            return Result.Failure<RoleDto>(Error.Conflict("Role"));

        var role = Role.Create(request.Name, request.Description);
        await roleRepository.AddAsync(role, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(MapToDto(role));
    }

    internal static RoleDto MapToDto(Role role) => new(
        role.Id, role.Name, role.Description, role.IsSystem, role.CreatedAt,
        role.RolePermissions.Select(rp => new PermissionDto(
            rp.Permission!.Id, rp.Permission.Code, rp.Permission.Name,
            rp.Permission.Description, rp.Permission.Module)));
}
