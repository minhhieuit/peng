using Peng.SharedKernel.Infrastructure;

namespace Peng.Modules.Members.Application;

public class MembersModuleDescriptor : IModuleDescriptor
{
    public string ModuleName => "Members";
    public string Description => "Public members who self-register through the client, separate from admin users";
    public string Version => "1.0.0";

    public IEnumerable<PermissionDescriptor> Permissions => MembersPermissions.All
        .Select(p => new PermissionDescriptor(p.Code, p.Name, p.Description, "Members"));

    public IEnumerable<FeatureDescriptor> Features =>
    [
        new("Member Self-Registration", "Visitors register a member account through the public client",
        [
            "Anyone can register a member account via the public client",
            "Member accounts are completely separate from admin users and carry no admin permissions",
            "A member token cannot be used against admin endpoints, and vice versa",
        ]),
        new("Member Authentication", "Members log in and receive a member-scoped JWT",
        [
            "Members authenticate at the member login endpoint, not the admin login endpoint",
            "Disabled member accounts cannot log in",
        ]),
        new("Admin-Managed Members", "Admins create and deactivate member accounts manually",
        [
            "Only users with 'members:members:write' can create a member or deactivate one",
            "Only users with 'members:members:read' can view the member list",
            "Manually created members get a temporary password and must change it on first use",
        ]),
    ];
}
