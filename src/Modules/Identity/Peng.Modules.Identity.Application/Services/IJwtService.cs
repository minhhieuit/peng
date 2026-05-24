using Peng.Modules.Identity.Domain.Entities;

namespace Peng.Modules.Identity.Application.Services;

public interface IJwtService
{
    string GenerateToken(User user, IEnumerable<string> permissions);
}
