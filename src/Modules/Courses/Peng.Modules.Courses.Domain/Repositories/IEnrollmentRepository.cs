using Peng.Modules.Courses.Domain.Entities;

namespace Peng.Modules.Courses.Domain.Repositories;

public interface IEnrollmentRepository
{
    Task<Enrollment?> GetAsync(Guid courseId, Guid userId, CancellationToken ct = default);
    Task<List<Enrollment>> GetByCourseAsync(Guid courseId, CancellationToken ct = default);
    Task<List<Enrollment>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Enrollment enrollment, CancellationToken ct = default);
}
