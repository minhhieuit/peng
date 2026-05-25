using Peng.Modules.Courses.Application.DTOs;
using Peng.Modules.Courses.Domain.Entities;
using Peng.Modules.Courses.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.CreateCourse;

public class CreateCourseCommandHandler(ICourseRepository courseRepository, ICoursesUnitOfWork uow)
    : ICommandHandler<CreateCourseCommand, CourseDto>
{
    public async Task<Result<CourseDto>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = Course.Create(request.Title, request.Description, request.Price, request.InstructorId);
        if (request.ThumbnailUrl is not null)
            course.Update(request.Title, request.Description, request.Price, request.ThumbnailUrl);

        await courseRepository.AddAsync(course, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return Result.Success(ToDto(course));
    }

    private static CourseDto ToDto(Course c) =>
        new(c.Id, c.Title, c.Description, c.ThumbnailUrl, c.Price, c.IsPublished, c.InstructorId, c.Enrollments.Count, c.CreatedAt);
}
