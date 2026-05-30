using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.UnenrollUser;

public record UnenrollUserCommand(Guid CourseId, Guid MemberId) : ICommand<bool>;
