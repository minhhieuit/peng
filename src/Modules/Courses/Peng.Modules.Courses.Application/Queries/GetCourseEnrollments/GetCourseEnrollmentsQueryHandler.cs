using Peng.Modules.Courses.Application.DTOs;
using Peng.Modules.Courses.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Queries.GetCourseEnrollments;

public class GetCourseEnrollmentsQueryHandler(
    ICourseRepository courseRepository,
    IEnrollmentRepository enrollmentRepository,
    IUserInfoProvider userInfo)
    : IQueryHandler<GetCourseEnrollmentsQuery, List<EnrollmentDto>>
{
    public async Task<Result<List<EnrollmentDto>>> Handle(GetCourseEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null) return Result.Failure<List<EnrollmentDto>>(Error.NotFound("Course"));

        var enrollments = await enrollmentRepository.GetByCourseAsync(request.CourseId, cancellationToken);
        var members = await userInfo.GetByIdsAsync(enrollments.Select(e => e.MemberId), cancellationToken);

        var dtos = enrollments.Select(e =>
        {
            members.TryGetValue(e.MemberId, out var m);
            return new EnrollmentDto(e.Id, e.CourseId, course.Title, e.MemberId, m?.FullName, m?.Email, e.Status.ToString(), e.EnrolledAt);
        }).ToList();

        return Result.Success(dtos);
    }
}
