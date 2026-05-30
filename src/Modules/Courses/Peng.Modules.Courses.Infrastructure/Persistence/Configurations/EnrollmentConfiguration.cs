using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Peng.Modules.Courses.Domain.Entities;

namespace Peng.Modules.Courses.Infrastructure.Persistence.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        builder.HasIndex(e => new { e.CourseId, e.MemberId }).IsUnique();
    }
}
