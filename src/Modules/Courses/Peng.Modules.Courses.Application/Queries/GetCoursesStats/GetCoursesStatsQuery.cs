using Peng.Modules.Courses.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Queries.GetCoursesStats;

public record GetCoursesStatsQuery : IQuery<CourseStatsDto>;
