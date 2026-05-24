namespace Peng.Modules.Identity.Application.DTOs;

public record AuthResponse(string AccessToken, string TokenType, int ExpiresIn, UserDto User);

public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    bool IsActive,
    DateTime CreatedAt,
    IEnumerable<string> Roles,
    IEnumerable<string> Permissions);
