using Peng.Modules.Members.Application.DTOs;
using Peng.Modules.Members.Domain.Entities;

namespace Peng.Modules.Members.Application;

internal static class MemberMappingExtensions
{
    public static MemberDto ToDto(this Member member) =>
        new(member.Id, member.Email, member.FirstName, member.LastName, member.FullName,
            member.IsActive, member.MustChangePassword, member.CreatedAt, member.LastLoginAt);
}
