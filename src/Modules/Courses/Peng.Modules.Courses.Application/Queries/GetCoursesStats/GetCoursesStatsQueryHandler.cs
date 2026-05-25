using Peng.Modules.Courses.Application.DTOs;
using Peng.Modules.Courses.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Queries.GetCoursesStats;

public class GetCoursesStatsQueryHandler(ICourseRepository courseRepository)
    : IQueryHandler<GetCoursesStatsQuery, CourseStatsDto>
{
    public async Task<Result<CourseStatsDto>> Handle(GetCoursesStatsQuery request, CancellationToken cancellationToken)
    {
        var (total, published, drafts, enrollments) = await courseRepository.GetStatisticsAsync(cancellationToken);
        return Result.Success(new CourseStatsDto(total, published, drafts, enrollments));
    }
}
