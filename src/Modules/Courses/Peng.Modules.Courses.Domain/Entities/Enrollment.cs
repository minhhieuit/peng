using Peng.SharedKernel.Domain;

namespace Peng.Modules.Courses.Domain.Entities;

public class Enrollment : Entity
{
    private Enrollment() { }

    public Guid CourseId { get; private set; }
    public Guid MemberId { get; private set; }
    public EnrollmentStatus Status { get; private set; }
    public DateTime EnrolledAt { get; private set; }

    public static Enrollment Create(Guid courseId, Guid memberId)
        => new() { CourseId = courseId, MemberId = memberId, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow };

    public void Cancel() => Status = EnrollmentStatus.Cancelled;
    public void Reactivate() => Status = EnrollmentStatus.Active;
}
