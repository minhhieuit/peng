using Peng.Modules.Courses.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.PublishCourse;

public record PublishCourseCommand(Guid Id) : ICommand<CourseDto>;
