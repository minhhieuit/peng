namespace Peng.Modules.Identity.Application;

public static class IdentityPermissions
{
    public const string UsersRead = "identity:users:read";
    public const string UsersWrite = "identity:users:write";
    public const string UsersDelete = "identity:users:delete";
    public const string RolesRead = "identity:roles:read";
    public const string RolesWrite = "identity:roles:write";
    public const string PermissionsRead = "identity:permissions:read";

    public static IEnumerable<(string Code, string Name, string Description)> All =>
    [
        (UsersRead, "View Users", "View user list and user details"),
        (UsersWrite, "Manage Users", "Create, update, and assign roles to users"),
        (UsersDelete, "Delete Users", "Deactivate or delete users"),
        (RolesRead, "View Roles", "View role list and role permissions"),
        (RolesWrite, "Manage Roles", "Create, update roles and assign permissions"),
        (PermissionsRead, "View Permissions", "View all permissions"),
    ];
}
