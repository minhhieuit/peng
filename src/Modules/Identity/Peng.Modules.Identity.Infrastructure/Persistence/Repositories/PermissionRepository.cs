using Microsoft.EntityFrameworkCore;
using Peng.Modules.Identity.Domain.Entities;
using Peng.Modules.Identity.Domain.Repositories;

namespace Peng.Modules.Identity.Infrastructure.Persistence.Repositories;

internal class PermissionRepository(IdentityDbContext context) : IPermissionRepository
{
    public async Task<Permission?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        await context.Permissions.FirstOrDefaultAsync(p => p.Code == code, ct);

    public async Task<IReadOnlyList<Permission>> GetAllAsync(CancellationToken ct = default) =>
        await context.Permissions.OrderBy(p => p.Module).ThenBy(p => p.Code).ToListAsync(ct);

    public async Task<IReadOnlyList<Permission>> GetByCodesAsync(IEnumerable<string> codes, CancellationToken ct = default) =>
        await context.Permissions.Where(p => codes.Contains(p.Code)).ToListAsync(ct);

    public async Task AddRangeAsync(IEnumerable<Permission> permissions, CancellationToken ct = default) =>
        await context.Permissions.AddRangeAsync(permissions, ct);

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default) =>
        await context.Permissions.AnyAsync(p => p.Code == code, ct);
}
