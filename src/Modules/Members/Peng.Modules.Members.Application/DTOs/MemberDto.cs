namespace Peng.Modules.Members.Application.DTOs;

public record MemberDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    bool IsActive,
    bool MustChangePassword,
    DateTime CreatedAt,
    DateTime? LastLoginAt);

public record MemberAuthResponse(string AccessToken, string TokenType, int ExpiresIn, MemberDto Member);

public record CreateMemberResponse(Guid Id, string Email, string FullName, string TemporaryPassword);
