using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Peng.Modules.Members.Domain.Entities;

namespace Peng.Modules.Members.Infrastructure.Persistence.Configurations;

internal class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Email).IsRequired().HasMaxLength(256);
        builder.HasIndex(m => m.Email).IsUnique();
        builder.Property(m => m.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(m => m.LastName).IsRequired().HasMaxLength(100);
        builder.Property(m => m.PasswordHash).IsRequired();

        builder.Ignore(m => m.DomainEvents);
    }
}
