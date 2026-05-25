using Microsoft.EntityFrameworkCore;
using Peng.Modules.Identity.Infrastructure.Persistence;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Infrastructure.Services;

public class UserInfoProvider(IdentityDbContext context) : IUserInfoProvider
{
    public async Task<IReadOnlyDictionary<Guid, UserInfo>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var idSet = ids.Distinct().ToArray();
        if (idSet.Length == 0) return new Dictionary<Guid, UserInfo>();

        var users = await context.Users
            .Where(u => idSet.Contains(u.Id))
            .Select(u => new { u.Id, u.FirstName, u.LastName, u.Email })
            .ToListAsync(ct);

        return users.ToDictionary(
            u => u.Id,
            u => new UserInfo(u.Id, $"{u.FirstName} {u.LastName}", u.Email));
    }
}
