using Peng.Modules.Courses.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.DeleteCourse;

public class DeleteCourseCommandHandler(ICourseRepository courseRepository, ICoursesUnitOfWork uow)
    : ICommandHandler<DeleteCourseCommand, bool>
{
    public async Task<Result<bool>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (course is null) return Result.Failure<bool>(Error.NotFound("Course"));

        courseRepository.Remove(course);
        await uow.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }
}
