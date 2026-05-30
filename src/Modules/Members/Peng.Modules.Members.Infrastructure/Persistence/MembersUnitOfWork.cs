using Peng.Modules.Members.Domain.Repositories;

namespace Peng.Modules.Members.Infrastructure.Persistence;

public class MembersUnitOfWork(MembersDbContext context) : IMembersUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct) => context.SaveChangesAsync(ct);
}
