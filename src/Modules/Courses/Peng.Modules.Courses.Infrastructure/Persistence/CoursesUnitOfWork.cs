using Peng.Modules.Courses.Domain.Repositories;

namespace Peng.Modules.Courses.Infrastructure.Persistence;

public class CoursesUnitOfWork(CoursesDbContext context) : ICoursesUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct) => context.SaveChangesAsync(ct);
}
