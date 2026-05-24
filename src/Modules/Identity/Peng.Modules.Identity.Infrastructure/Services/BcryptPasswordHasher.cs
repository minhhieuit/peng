using Peng.Modules.Identity.Application.Services;

namespace Peng.Modules.Identity.Infrastructure.Services;

internal class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}
