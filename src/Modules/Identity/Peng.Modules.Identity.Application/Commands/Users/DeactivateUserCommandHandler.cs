using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Users;

internal sealed class DeactivateUserCommandHandler(
    IUserRepository userRepository,
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork) : ICommandHandler<DeactivateUserCommand>
{
    public async Task<Result> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId == request.Id)
            return Result.Failure(new Error("User.SelfDeactivate", "You cannot deactivate your own account."));

        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null) return Result.Failure(Error.NotFound("User"));

        user.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
