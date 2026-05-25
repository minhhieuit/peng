using Peng.Modules.Courses.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.UpdateCourse;

public record UpdateCourseCommand(Guid Id, string Title, string Description, decimal Price, string? ThumbnailUrl) : ICommand<CourseDto>;
