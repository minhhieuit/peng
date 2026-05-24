using Peng.SharedKernel.Domain;

namespace Peng.Modules.Identity.Domain.Entities;

public class Role : Entity
{
    private readonly List<RolePermission> _rolePermissions = [];

    private Role() { }

    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public bool IsSystem { get; private set; }
    public IReadOnlyList<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    public static Role Create(string name, string description, bool isSystem = false)
    {
        return new Role { Name = name, Description = description, IsSystem = isSystem };
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
        SetUpdatedAt();
    }

    public void AssignPermission(Permission permission)
    {
        if (_rolePermissions.Any(rp => rp.PermissionId == permission.Id)) return;
        _rolePermissions.Add(new RolePermission(Id, permission.Id));
        SetUpdatedAt();
    }

    public void RemovePermission(Guid permissionId)
    {
        var rp = _rolePermissions.FirstOrDefault(rp => rp.PermissionId == permissionId);
        if (rp is not null) _rolePermissions.Remove(rp);
        SetUpdatedAt();
    }
}
