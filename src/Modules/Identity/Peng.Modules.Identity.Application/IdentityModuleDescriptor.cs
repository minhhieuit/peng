using Peng.SharedKernel.Infrastructure;

namespace Peng.Modules.Identity.Application;

/// <summary>
/// Self-documents the Identity module for onboarding and documentation generation.
/// </summary>
public class IdentityModuleDescriptor : IModuleDescriptor
{
    public string ModuleName => "Identity";
    public string Description => "Handles authentication, user management, roles, and permissions.";
    public string Version => "1.0.0";

    public IEnumerable<PermissionDescriptor> Permissions => IdentityPermissions.All
        .Select(p => new PermissionDescriptor(p.Code, p.Name, p.Description, "Identity"));

    public IEnumerable<FeatureDescriptor> Features =>
    [
        new("Registration", "Allow new users to create an account",
        [
            "Email must be unique across the system",
            "Password must meet complexity requirements (8+ chars, uppercase, lowercase, digit)",
            "New users are automatically assigned the 'Member' role",
        ]),
        new("Login", "Authenticate an existing user and receive a JWT token",
        [
            "Returns JWT token valid for 1 hour",
            "Inactive accounts cannot login",
            "Invalid credentials return same error as not-found (prevents enumeration)",
        ]),
        new("User Management", "CRUD operations on users and role assignments",
        [
            "Only users with 'identity:users:write' permission can manage users",
            "Users cannot delete themselves",
        ]),
        new("Role & Permission Management", "Manage roles and their permission sets",
        [
            "System roles cannot be deleted",
            "Permissions are defined at compile time per module",
            "Roles aggregate permissions; users aggregate roles",
        ]),
    ];
}
