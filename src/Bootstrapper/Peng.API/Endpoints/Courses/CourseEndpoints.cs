using MediatR;
using Peng.Modules.Courses.Application.Commands.EnrollUser;
using Peng.Modules.Courses.Application.Commands.UnenrollUser;
using Peng.Modules.Courses.Application.Queries.GetCourseById;
using Peng.Modules.Courses.Application.Queries.GetCourses;
using Peng.Modules.Courses.Application.Queries.GetMyEnrollments;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Endpoints.Courses;

public class CourseEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/courses").WithTags("Courses");

        // Public
        group.MapGet("", async (IMediator mediator, int page = 1, int pageSize = 20) =>
        {
            var result = await mediator.Send(new GetCoursesQuery(page, pageSize, PublishedOnly: true));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Problem(result.Error.Description);
        });

        group.MapGet("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCourseByIdQuery(id, RequirePublished: true));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        });

        // Member-only: enroll / unenroll / my-enrollments. The "sub" claim is the member id.
        group.MapPost("{id:guid}/enroll", async (Guid id, IMediator mediator, HttpContext ctx) =>
        {
            var memberId = GetMemberId(ctx);
            if (memberId is null) return Results.Unauthorized();
            var result = await mediator.Send(new EnrollUserCommand(id, memberId.Value));
            return result.IsSuccess ? Results.Ok(result.Value)
                : result.Error.Code.Contains("Conflict") ? Results.Conflict(result.Error.Description)
                : Results.NotFound();
        }).RequireAuthorization("MemberToken");

        group.MapDelete("{id:guid}/enroll", async (Guid id, IMediator mediator, HttpContext ctx) =>
        {
            var memberId = GetMemberId(ctx);
            if (memberId is null) return Results.Unauthorized();
            var result = await mediator.Send(new UnenrollUserCommand(id, memberId.Value));
            return result.IsSuccess ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization("MemberToken");

        group.MapGet("my-enrollments", async (IMediator mediator, HttpContext ctx) =>
        {
            var memberId = GetMemberId(ctx);
            if (memberId is null) return Results.Unauthorized();
            var result = await mediator.Send(new GetMyEnrollmentsQuery(memberId.Value));
            return Results.Ok(result.Value);
        }).RequireAuthorization("MemberToken");
    }

    private static Guid? GetMemberId(HttpContext ctx)
    {
        var sub = ctx.User.FindFirst("sub")?.Value;
        return Guid.TryParse(sub, out var id) ? id : null;
    }
}
