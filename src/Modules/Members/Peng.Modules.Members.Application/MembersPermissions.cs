namespace Peng.Modules.Members.Application;

public static class MembersPermissions
{
    public const string MembersRead = "members:members:read";
    public const string MembersWrite = "members:members:write";

    public static IEnumerable<(string Code, string Name, string Description)> All =>
    [
        (MembersRead, "View Members", "View member list and member details"),
        (MembersWrite, "Manage Members", "Create members manually and deactivate members"),
    ];
}
