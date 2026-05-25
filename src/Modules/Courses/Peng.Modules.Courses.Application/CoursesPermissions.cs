namespace Peng.Modules.Courses.Application;

public static class CoursesPermissions
{
    public const string CoursesRead = "courses:courses:read";
    public const string CoursesWrite = "courses:courses:write";
    public const string CoursesDelete = "courses:courses:delete";
    public const string EnrollmentsRead = "courses:enrollments:read";

    public static IEnumerable<(string Code, string Name, string Description)> All =>
    [
        (CoursesRead, "View Courses", "View course list and course details"),
        (CoursesWrite, "Manage Courses", "Create, update, and publish courses"),
        (CoursesDelete, "Delete Courses", "Delete courses"),
        (EnrollmentsRead, "View Enrollments", "View course enrollment lists"),
    ];
}
