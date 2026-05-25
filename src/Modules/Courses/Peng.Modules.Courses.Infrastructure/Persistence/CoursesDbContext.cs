using Microsoft.EntityFrameworkCore;
using Peng.Modules.Courses.Domain.Entities;

namespace Peng.Modules.Courses.Infrastructure.Persistence;

public class CoursesDbContext(DbContextOptions<CoursesDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("courses");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoursesDbContext).Assembly);
    }
}
