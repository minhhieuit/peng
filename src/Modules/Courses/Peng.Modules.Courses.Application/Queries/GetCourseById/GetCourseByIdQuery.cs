using Peng.Modules.Courses.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Queries.GetCourseById;

public record GetCourseByIdQuery(Guid Id, bool RequirePublished = true) : IQuery<CourseDto>;
