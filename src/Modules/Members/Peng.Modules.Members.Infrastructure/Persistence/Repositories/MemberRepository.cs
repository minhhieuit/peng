using Microsoft.EntityFrameworkCore;
using Peng.Modules.Members.Domain.Entities;
using Peng.Modules.Members.Domain.Repositories;

namespace Peng.Modules.Members.Infrastructure.Persistence.Repositories;

internal class MemberRepository(MembersDbContext context) : IMemberRepository
{
    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Members.FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<Member?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await context.Members.FirstOrDefaultAsync(m => m.Email == email.ToLowerInvariant(), ct);

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
        await context.Members.AnyAsync(m => m.Email == email.ToLowerInvariant(), ct);

    public async Task AddAsync(Member member, CancellationToken ct = default) =>
        await context.Members.AddAsync(member, ct);

    public async Task<(IReadOnlyList<Member> Members, int Total)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = context.Members.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(m => m.Email.Contains(search) || m.FirstName.Contains(search) || m.LastName.Contains(search));

        var total = await query.CountAsync(ct);
        var members = await query.OrderBy(m => m.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (members, total);
    }
}
