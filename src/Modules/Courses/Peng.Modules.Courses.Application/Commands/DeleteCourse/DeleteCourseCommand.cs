using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.DeleteCourse;

public record DeleteCourseCommand(Guid Id) : ICommand<bool>;
