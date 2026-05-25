using Peng.Modules.Courses.Domain.Entities;

namespace Peng.Modules.Courses.Domain.Repositories;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Course?> GetByIdWithEnrollmentsAsync(Guid id, CancellationToken ct = default);
    Task<(List<Course> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, bool publishedOnly, CancellationToken ct = default);
    Task AddAsync(Course course, CancellationToken ct = default);
    void Remove(Course course);
}
