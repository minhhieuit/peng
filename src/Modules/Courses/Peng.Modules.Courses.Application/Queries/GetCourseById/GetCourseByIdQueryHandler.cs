using Peng.Modules.Courses.Application.DTOs;
using Peng.Modules.Courses.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Queries.GetCourseById;

public class GetCourseByIdQueryHandler(ICourseRepository courseRepository)
    : IQueryHandler<GetCourseByIdQuery, CourseDto>
{
    public async Task<Result<CourseDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByIdWithEnrollmentsAsync(request.Id, cancellationToken);
        if (course is null) return Result.Failure<CourseDto>(Error.NotFound("Course"));
        if (request.RequirePublished && !course.IsPublished) return Result.Failure<CourseDto>(Error.NotFound("Course"));
        return Result.Success(new CourseDto(course.Id, course.Title, course.Description, course.ThumbnailUrl, course.Price, course.IsPublished, course.InstructorId, course.Enrollments.Count, course.CreatedAt));
    }
}
