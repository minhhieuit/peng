using Peng.Modules.Courses.Domain.Entities;

namespace Peng.Modules.Courses.Domain.Repositories;

public interface IEnrollmentRepository
{
    Task<Enrollment?> GetAsync(Guid courseId, Guid memberId, CancellationToken ct = default);
    Task<List<Enrollment>> GetByCourseAsync(Guid courseId, CancellationToken ct = default);
    Task<List<Enrollment>> GetByMemberAsync(Guid memberId, CancellationToken ct = default);
    Task AddAsync(Enrollment enrollment, CancellationToken ct = default);
}
