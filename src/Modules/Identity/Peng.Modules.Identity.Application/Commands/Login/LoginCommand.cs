using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;
