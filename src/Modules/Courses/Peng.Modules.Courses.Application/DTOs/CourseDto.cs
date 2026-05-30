namespace Peng.Modules.Courses.Application.DTOs;

public record CourseDto(
    Guid Id,
    string Title,
    string Description,
    string? ThumbnailUrl,
    decimal Price,
    bool IsPublished,
    Guid InstructorId,
    int EnrollmentCount,
    DateTime CreatedAt
);

public record EnrollmentDto(
    Guid Id,
    Guid CourseId,
    string CourseTitle,
    Guid MemberId,
    string? MemberName,
    string? MemberEmail,
    string Status,
    DateTime EnrolledAt
);

public record CourseStatsDto(int Total, int Published, int Drafts, int TotalEnrollments);
