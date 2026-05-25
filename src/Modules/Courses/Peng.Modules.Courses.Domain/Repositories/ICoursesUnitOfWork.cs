namespace Peng.Modules.Courses.Domain.Repositories;

public interface ICoursesUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
