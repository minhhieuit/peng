using Peng.Modules.Courses.Application.DTOs;
using Peng.Modules.Courses.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(ICourseRepository courseRepository, ICoursesUnitOfWork uow)
    : ICommandHandler<UpdateCourseCommand, CourseDto>
{
    public async Task<Result<CourseDto>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (course is null) return Result.Failure<CourseDto>(Error.NotFound("Course"));

        course.Update(request.Title, request.Description, request.Price, request.ThumbnailUrl);
        await uow.SaveChangesAsync(cancellationToken);

        return Result.Success(new CourseDto(course.Id, course.Title, course.Description, course.ThumbnailUrl, course.Price, course.IsPublished, course.InstructorId, course.Enrollments.Count, course.CreatedAt));
    }
}
