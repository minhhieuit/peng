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

    public async Task<(List<Course> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CourseListFilter filter, CancellationToken ct)
    {
        var query = context.Courses.Include(c => c.Enrollments).AsQueryable();

        if (filter.IsPublished is { } isPublished)
            query = query.Where(c => c.IsPublished == isPublished);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = $"%{filter.Search.Trim()}%";
            query = query.Where(c => EF.Functions.ILike(c.Title, term) || EF.Functions.ILike(c.Description, term));
        }

        var total = await query.CountAsync(ct);
        var items = await query.OrderByDescending(c => c.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task<(int Total, int Published, int Drafts, int TotalEnrollments)> GetStatisticsAsync(CancellationToken ct)
    {
        var total = await context.Courses.CountAsync(ct);
        var published = await context.Courses.CountAsync(c => c.IsPublished, ct);
        var enrollments = await context.Enrollments.CountAsync(e => e.Status == EnrollmentStatus.Active, ct);
        return (total, published, total - published, enrollments);
    }

    public async Task AddAsync(Course course, CancellationToken ct) => await context.Courses.AddAsync(course, ct);
    public void Remove(Course course) => context.Courses.Remove(course);
}
