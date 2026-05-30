namespace Peng.SharedKernel.Application;

/// <summary>
/// Token type claim values. Used by authorization policies to keep admin and
/// member tokens from being used against each other's endpoints.
/// </summary>
public static class TokenTypes
{
    public const string ClaimName = "token_type";
    public const string Admin = "admin";
    public const string Member = "member";
}

public interface IJwtService
{
    string GenerateToken(
        Guid subjectId,
        string email,
        string firstName,
        string lastName,
        IEnumerable<string> roles,
        IEnumerable<string> permissions,
        string tokenType);
}
