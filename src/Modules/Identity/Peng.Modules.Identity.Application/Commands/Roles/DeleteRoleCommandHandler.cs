using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Roles;

internal sealed class DeleteRoleCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (role is null) return Result.Failure(Error.NotFound("Role"));
        if (role.IsSystem) return Result.Failure(new Error("Role.System", "System roles cannot be deleted."));

        roleRepository.Remove(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
