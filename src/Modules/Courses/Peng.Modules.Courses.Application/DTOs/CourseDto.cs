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
    Guid UserId,
    string Status,
    DateTime EnrolledAt
);
