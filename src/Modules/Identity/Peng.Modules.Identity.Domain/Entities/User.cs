using Peng.SharedKernel.Domain;

namespace Peng.Modules.Identity.Domain.Entities;

public class User : Entity
{
    private readonly List<UserRole> _userRoles = [];

    private User() { }

    public string Email { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public DateTime? LastLoginAt { get; private set; }
    public IReadOnlyList<UserRole> UserRoles => _userRoles.AsReadOnly();

    public string FullName => $"{FirstName} {LastName}";

    public static User Create(string email, string firstName, string lastName, string passwordHash)
    {
        var user = new User
        {
            Email = email.ToLowerInvariant(),
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = passwordHash,
        };
        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id, user.Email));
        return user;
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        SetUpdatedAt();
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public void AssignRole(Role role)
    {
        if (_userRoles.Any(ur => ur.RoleId == role.Id)) return;
        _userRoles.Add(new UserRole(Id, role.Id));
        SetUpdatedAt();
    }

    public void RemoveRole(Guid roleId)
    {
        var userRole = _userRoles.FirstOrDefault(ur => ur.RoleId == roleId);
        if (userRole is not null) _userRoles.Remove(userRole);
        SetUpdatedAt();
    }

    public IEnumerable<string> GetAllPermissions() =>
        UserRoles.SelectMany(ur => ur.Role?.RolePermissions ?? [])
                 .Select(rp => rp.Permission?.Code ?? string.Empty)
                 .Where(p => !string.IsNullOrEmpty(p))
                 .Distinct();
}
