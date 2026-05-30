using Microsoft.EntityFrameworkCore;
using Peng.Modules.Members.Infrastructure.Persistence;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Infrastructure.Services;

/// <summary>
/// Resolves member display info for other modules (e.g. Courses enrollment lists)
/// without exposing the Members entity across the module boundary.
/// </summary>
public class MemberInfoProvider(MembersDbContext context) : IUserInfoProvider
{
    public async Task<IReadOnlyDictionary<Guid, UserInfo>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var idSet = ids.Distinct().ToArray();
        if (idSet.Length == 0) return new Dictionary<Guid, UserInfo>();

        var members = await context.Members
            .Where(m => idSet.Contains(m.Id))
            .Select(m => new { m.Id, m.FirstName, m.LastName, m.Email })
            .ToListAsync(ct);

        return members.ToDictionary(
            m => m.Id,
            m => new UserInfo(m.Id, $"{m.FirstName} {m.LastName}", m.Email));
    }
}
