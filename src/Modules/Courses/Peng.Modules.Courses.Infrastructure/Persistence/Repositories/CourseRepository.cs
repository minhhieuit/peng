using Microsoft.EntityFrameworkCore;
using Peng.Modules.Courses.Domain.Entities;
using Peng.Modules.Courses.Domain.Repositories;

namespace Peng.Modules.Courses.Infrastructure.Persistence.Repositories;

public class CourseRepository(CoursesDbContext context) : ICourseRepository
{
    public Task<Course?> GetByIdAsync(Guid id, CancellationToken ct) =>
        context.Courses.FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<Course?> GetByIdWithEnrollmentsAsync(Guid id, CancellationToken ct) =>
        context.Courses.Include(c => c.Enrollments).FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<(List<Course> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, bool publishedOnly, CancellationToken ct)
    {
        var query = context.Courses.Include(c => c.Enrollments).AsQueryable();
        if (publishedOnly) query = query.Where(c => c.IsPublished);
        var total = await query.CountAsync(ct);
        var items = await query.OrderByDescending(c => c.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task AddAsync(Course course, CancellationToken ct) => await context.Courses.AddAsync(course, ct);
    public void Remove(Course course) => context.Courses.Remove(course);
}
