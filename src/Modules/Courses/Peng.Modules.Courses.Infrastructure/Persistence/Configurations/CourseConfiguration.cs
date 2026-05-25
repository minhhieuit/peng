using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Peng.Modules.Courses.Domain.Entities;

namespace Peng.Modules.Courses.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(5000).IsRequired();
        builder.Property(c => c.ThumbnailUrl).HasMaxLength(500);
        builder.Property(c => c.Price).HasPrecision(18, 2);
        builder.HasMany(c => c.Enrollments).WithOne().HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
    }
}
