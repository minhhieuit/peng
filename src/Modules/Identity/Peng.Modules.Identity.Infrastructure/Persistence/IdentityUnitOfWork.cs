using Peng.Modules.Identity.Domain.Repositories;

namespace Peng.Modules.Identity.Infrastructure.Persistence;

internal class IdentityUnitOfWork(IdentityDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await context.SaveChangesAsync(ct);
}
