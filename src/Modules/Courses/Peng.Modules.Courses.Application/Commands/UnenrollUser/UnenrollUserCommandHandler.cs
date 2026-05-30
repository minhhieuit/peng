using Peng.Modules.Courses.Domain.Entities;
using Peng.Modules.Courses.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.UnenrollUser;

public class UnenrollUserCommandHandler(IEnrollmentRepository enrollmentRepository, ICoursesUnitOfWork uow)
    : ICommandHandler<UnenrollUserCommand, bool>
{
    public async Task<Result<bool>> Handle(UnenrollUserCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await enrollmentRepository.GetAsync(request.CourseId, request.MemberId, cancellationToken);
        if (enrollment is null || enrollment.Status == EnrollmentStatus.Cancelled)
            return Result.Failure<bool>(Error.NotFound("Enrollment"));

        enrollment.Cancel();
        await uow.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }
}
