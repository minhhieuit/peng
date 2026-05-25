using MediatR;
using Peng.Modules.Courses.Application;
using Peng.Modules.Courses.Application.Commands.CreateCourse;
using Peng.Modules.Courses.Application.Commands.DeleteCourse;
using Peng.Modules.Courses.Application.Commands.PublishCourse;
using Peng.Modules.Courses.Application.Commands.UnenrollUser;
using Peng.Modules.Courses.Application.Commands.UpdateCourse;
using Peng.Modules.Courses.Application.Queries.GetCourseById;
using Peng.Modules.Courses.Application.Queries.GetCourseEnrollments;
using Peng.Modules.Courses.Application.Queries.GetCourses;
using Peng.Modules.Courses.Application.Queries.GetCoursesStats;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints.Courses;

public class CourseManagementEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/manage/courses")
            .WithTags("Course Management")
            .RequireAuthorization(p => p.RequireClaim("permission", CoursesPermissions.CoursesRead));

        group.MapGet("", async (
            IMediator mediator,
            int page = 1,
            int pageSize = 20,
            string? search = null,
            CourseStatusFilter status = CourseStatusFilter.All) =>
        {
            var result = await mediator.Send(new GetCoursesQuery(page, pageSize, PublishedOnly: false, search, status));
            return Results.Ok(result.Value);
        });

        group.MapGet("stats", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCoursesStatsQuery());
            return Results.Ok(result.Value);
        });

        group.MapGet("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCourseByIdQuery(id, RequirePublished: false));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        });

        group.MapPost("", async (CreateCourseCommand cmd, IMediator mediator, HttpContext ctx) =>
        {
            var instructorId = GetUserId(ctx) ?? Guid.Empty;
            var result = await mediator.Send(cmd with { InstructorId = instructorId });
            if (!result.IsSuccess)
                return result.Error.Code.StartsWith("Validation") ? Results.UnprocessableEntity(result.Error) : Results.BadRequest(result.Error);
            return Results.Created($"/api/manage/courses/{result.Value!.Id}", result.Value);
        }).RequireAuthorization(p => p.RequireClaim("permission", CoursesPermissions.CoursesWrite));

        group.MapPut("{id:guid}", async (Guid id, UpdateCourseCommand cmd, IMediator mediator) =>
        {
            var result = await mediator.Send(cmd with { Id = id });
            if (!result.IsSuccess)
                return result.Error.Code.StartsWith("Validation") ? Results.UnprocessableEntity(result.Error) : Results.NotFound();
            return Results.Ok(result.Value);
        }).RequireAuthorization(p => p.RequireClaim("permission", CoursesPermissions.CoursesWrite));

        group.MapDelete("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteCourseCommand(id));
            return result.IsSuccess ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization(p => p.RequireClaim("permission", CoursesPermissions.CoursesDelete));

        group.MapPatch("{id:guid}/publish", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new PublishCourseCommand(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }).RequireAuthorization(p => p.RequireClaim("permission", CoursesPermissions.CoursesWrite));

        group.MapGet("{id:guid}/enrollments", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCourseEnrollmentsQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }).RequireAuthorization(p => p.RequireClaim("permission", CoursesPermissions.EnrollmentsRead));

        group.MapDelete("{id:guid}/enrollments/{userId:guid}", async (Guid id, Guid userId, IMediator mediator) =>
        {
            var result = await mediator.Send(new UnenrollUserCommand(id, userId));
            return result.IsSuccess ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization(p => p.RequireClaim("permission", CoursesPermissions.EnrollmentsWrite));
    }

    private static Guid? GetUserId(HttpContext ctx)
    {
        var sub = ctx.User.FindFirst("sub")?.Value;
        return Guid.TryParse(sub, out var id) ? id : null;
    }
}
