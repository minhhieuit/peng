using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Users;

public record UpdateUserCommand(Guid Id, string FirstName, string LastName) : ICommand<UserDto>;
