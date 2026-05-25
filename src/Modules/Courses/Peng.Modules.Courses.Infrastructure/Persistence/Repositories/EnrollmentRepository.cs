using Microsoft.EntityFrameworkCore;
using Peng.Modules.Courses.Domain.Entities;
using Peng.Modules.Courses.Domain.Repositories;

namespace Peng.Modules.Courses.Infrastructure.Persistence.Repositories;

public class EnrollmentRepository(CoursesDbContext context) : IEnrollmentRepository
{
    public Task<Enrollment?> GetAsync(Guid courseId, Guid userId, CancellationToken ct) =>
        context.Enrollments.FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId, ct);

    public Task<List<Enrollment>> GetByCourseAsync(Guid courseId, CancellationToken ct) =>
        context.Enrollments.Where(e => e.CourseId == courseId).ToListAsync(ct);

    public Task<List<Enrollment>> GetByUserAsync(Guid userId, CancellationToken ct) =>
        context.Enrollments.Where(e => e.UserId == userId).ToListAsync(ct);

    public async Task AddAsync(Enrollment enrollment, CancellationToken ct) =>
        await context.Enrollments.AddAsync(enrollment, ct);
}
