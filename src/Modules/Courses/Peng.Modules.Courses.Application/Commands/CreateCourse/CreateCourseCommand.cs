using Peng.Modules.Courses.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Commands.CreateCourse;

public record CreateCourseCommand(string Title, string Description, decimal Price, string? ThumbnailUrl, Guid InstructorId) : ICommand<CourseDto>;
