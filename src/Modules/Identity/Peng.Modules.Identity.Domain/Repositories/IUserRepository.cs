using Peng.Modules.Identity.Domain.Entities;

namespace Peng.Modules.Identity.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    Task<(IReadOnlyList<User> Users, int Total)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
}
