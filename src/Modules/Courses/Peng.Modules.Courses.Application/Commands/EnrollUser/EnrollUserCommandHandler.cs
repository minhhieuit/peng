using Peng.Modules.Courses.Application.DTOs;
using Peng.Modules.Courses.Domain.Entities;
using Peng.Modules.Courses.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.EnrollUser;

public class EnrollUserCommandHandler(ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository, ICoursesUnitOfWork uow)
    : ICommandHandler<EnrollUserCommand, EnrollmentDto>
{
    public async Task<Result<EnrollmentDto>> Handle(EnrollUserCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null) return Result.Failure<EnrollmentDto>(Error.NotFound("Course"));
        if (!course.IsPublished) return Result.Failure<EnrollmentDto>(Error.NotFound("Course"));

        var existing = await enrollmentRepository.GetAsync(request.CourseId, request.UserId, cancellationToken);
        if (existing is not null)
        {
            if (existing.Status == EnrollmentStatus.Active)
                return Result.Failure<EnrollmentDto>(Error.Conflict("Already enrolled in this course"));
            existing.Reactivate();
            await uow.SaveChangesAsync(cancellationToken);
            return Result.Success(new EnrollmentDto(existing.Id, existing.CourseId, course.Title, existing.UserId, existing.Status.ToString(), existing.EnrolledAt));
        }

        var enrollment = Enrollment.Create(request.CourseId, request.UserId);
        await enrollmentRepository.AddAsync(enrollment, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return Result.Success(new EnrollmentDto(enrollment.Id, enrollment.CourseId, course.Title, enrollment.UserId, enrollment.Status.ToString(), enrollment.EnrolledAt));
    }
}
