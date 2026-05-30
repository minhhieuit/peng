using Peng.Modules.Members.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.MemberLogin;

public record MemberLoginCommand(string Email, string Password) : ICommand<MemberAuthResponse>;
