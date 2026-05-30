using Peng.Modules.Members.Domain.Entities;

namespace Peng.Modules.Members.Domain.Repositories;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Member?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(Member member, CancellationToken ct = default);
    Task<(IReadOnlyList<Member> Members, int Total)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
}
