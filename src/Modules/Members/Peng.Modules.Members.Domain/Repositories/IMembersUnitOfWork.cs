namespace Peng.Modules.Members.Domain.Repositories;

public interface IMembersUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}
