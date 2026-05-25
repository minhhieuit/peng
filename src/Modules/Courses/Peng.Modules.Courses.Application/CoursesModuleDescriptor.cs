using Peng.SharedKernel.Infrastructure;

namespace Peng.Modules.Courses.Application;

public class CoursesModuleDescriptor : IModuleDescriptor
{
    public string ModuleName => "Courses";
    public string Description => "Course management and user enrollment system";
    public string Version => "1.0.0";

    public IEnumerable<PermissionDescriptor> Permissions => CoursesPermissions.All
        .Select(p => new PermissionDescriptor(p.Code, p.Name, p.Description, "Courses"));

    public IEnumerable<FeatureDescriptor> Features =>
    [
        new("Course Management", "Create, update, publish and delete courses",
        [
            "Only users with 'courses:courses:write' permission can create or update courses",
            "Only users with 'courses:courses:delete' permission can delete courses",
            "Courses must be published before users can enroll",
        ]),
        new("Enrollment Management", "View and manage course enrollments",
        [
            "Only users with 'courses:enrollments:read' permission can view enrollment lists",
        ]),
        new("User Enrollment", "Authenticated users can enroll in and unenroll from published courses",
        [
            "Users can only enroll in published courses",
            "A user cannot enroll in the same course twice while already active",
            "Cancelled enrollments can be reactivated by re-enrolling",
        ]),
    ];
}
