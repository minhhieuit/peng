using Microsoft.EntityFrameworkCore;
using Peng.Modules.Members.Domain.Entities;

namespace Peng.Modules.Members.Infrastructure.Persistence;

public class MembersDbContext(DbContextOptions<MembersDbContext> options) : DbContext(options)
{
    public DbSet<Member> Members => Set<Member>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("members");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MembersDbContext).Assembly);
    }
}
