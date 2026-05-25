using Peng.Modules.Courses.Application.DTOs;
using Peng.Modules.Courses.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Queries.GetCourses;

public class GetCoursesQueryHandler(ICourseRepository courseRepository)
    : IQueryHandler<GetCoursesQuery, PagedList<CourseDto>>
{
    public async Task<Result<PagedList<CourseDto>>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await courseRepository.GetPagedAsync(request.Page, request.PageSize, request.PublishedOnly, cancellationToken);
        var dtos = items.Select(c => new CourseDto(c.Id, c.Title, c.Description, c.ThumbnailUrl, c.Price, c.IsPublished, c.InstructorId, c.Enrollments.Count, c.CreatedAt)).ToList();
        return Result.Success(new PagedList<CourseDto>(dtos, request.Page, request.PageSize, total));
    }
}
