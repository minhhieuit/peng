using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Roles;

internal sealed class AssignPermissionsToRoleCommandHandler(
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AssignPermissionsToRoleCommand, RoleDto>
{
    public async Task<Result<RoleDto>> Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null) return Result.Failure<RoleDto>(Error.NotFound("Role"));

        var codes = request.PermissionCodes.ToList();
        var permissions = await permissionRepository.GetByCodesAsync(codes, cancellationToken);

        // Remove all existing, re-assign selected
        foreach (var rp in role.RolePermissions.ToList())
            role.RemovePermission(rp.PermissionId);

        foreach (var perm in permissions)
            role.AssignPermission(perm);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(CreateRoleCommandHandler.MapToDto(role));
    }
}
