using Peng.Modules.Courses.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.EnrollUser;

public record EnrollUserCommand(Guid CourseId, Guid UserId) : ICommand<EnrollmentDto>;
