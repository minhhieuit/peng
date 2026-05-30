using Peng.Modules.Members.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.RegisterMember;

public record RegisterMemberCommand(string Email, string Password, string FirstName, string LastName)
    : ICommand<MemberAuthResponse>;
