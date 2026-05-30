using Peng.Modules.Members.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.CreateMember;

/// <summary>Admin creates a member account manually. <paramref name="CreatedByUserId"/> is the acting admin.</summary>
public record CreateMemberCommand(string Email, string FirstName, string LastName, Guid CreatedByUserId)
    : ICommand<CreateMemberResponse>;
