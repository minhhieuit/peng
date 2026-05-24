using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Roles;

internal sealed class UpdateRoleCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateRoleCommand, RoleDto>
{
    public async Task<Result<RoleDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (role is null) return Result.Failure<RoleDto>(Error.NotFound("Role"));
        if (role.IsSystem) return Result.Failure<RoleDto>(new Error("Role.System", "System roles cannot be modified."));

        var duplicate = await roleRepository.GetByNameAsync(request.Name, cancellationToken);
        if (duplicate is not null && duplicate.Id != role.Id)
            return Result.Failure<RoleDto>(Error.Conflict("Role"));

        role.Update(request.Name, request.Description);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(CreateRoleCommandHandler.MapToDto(role));
    }
}
