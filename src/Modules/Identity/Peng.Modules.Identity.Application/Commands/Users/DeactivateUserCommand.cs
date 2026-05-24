using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Users;

public record DeactivateUserCommand(Guid Id) : ICommand;
