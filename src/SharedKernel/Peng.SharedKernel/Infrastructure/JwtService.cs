using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Peng.SharedKernel.Application;

namespace Peng.SharedKernel.Infrastructure;

public class JwtService(IOptions<JwtSettings> options) : IJwtService
{
    private readonly JwtSettings _settings = options.Value;

    public string GenerateToken(
        Guid subjectId,
        string email,
        string firstName,
        string lastName,
        IEnumerable<string> roles,
        IEnumerable<string> permissions,
        string tokenType)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, subjectId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.GivenName, firstName),
            new(JwtRegisteredClaimNames.FamilyName, lastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(TokenTypes.ClaimName, tokenType),
        };

        claims.AddRange(roles.Where(r => !string.IsNullOrEmpty(r)).Select(r => new Claim(ClaimTypes.Role, r)));
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(_settings.ExpirationSeconds),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
