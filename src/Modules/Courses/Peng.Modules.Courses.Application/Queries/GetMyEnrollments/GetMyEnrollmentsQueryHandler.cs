using Peng.Modules.Courses.Application.DTOs;
using Peng.Modules.Courses.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Queries.GetMyEnrollments;

public class GetMyEnrollmentsQueryHandler(IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository)
    : IQueryHandler<GetMyEnrollmentsQuery, List<EnrollmentDto>>
{
    public async Task<Result<List<EnrollmentDto>>> Handle(GetMyEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var enrollments = await enrollmentRepository.GetByMemberAsync(request.MemberId, cancellationToken);
        var result = new List<EnrollmentDto>();
        foreach (var e in enrollments)
        {
            var course = await courseRepository.GetByIdAsync(e.CourseId, cancellationToken);
            result.Add(new EnrollmentDto(e.Id, e.CourseId, course?.Title ?? "Unknown", e.MemberId, null, null, e.Status.ToString(), e.EnrolledAt));
        }
        return Result.Success(result);
    }
}
