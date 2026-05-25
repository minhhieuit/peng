using Peng.Modules.Courses.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Queries.GetCourses;

public enum CourseStatusFilter { All, Published, Draft }

public record GetCoursesQuery(
    int Page = 1,
    int PageSize = 20,
    bool PublishedOnly = true,
    string? Search = null,
    CourseStatusFilter Status = CourseStatusFilter.All
) : IQuery<PagedList<CourseDto>>;
