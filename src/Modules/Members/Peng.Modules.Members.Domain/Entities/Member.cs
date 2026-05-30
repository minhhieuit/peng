using Peng.SharedKernel.Domain;

namespace Peng.Modules.Members.Domain.Entities;

public class Member : Entity
{
    private Member() { }

    public string Email { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public bool MustChangePassword { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>Admin user who created this member manually; null for self-registered members.</summary>
    public Guid? CreatedByUserId { get; private set; }

    public string FullName => $"{FirstName} {LastName}";

    /// <summary>Self-registration via the public client.</summary>
    public static Member Register(string email, string firstName, string lastName, string passwordHash) =>
        new()
        {
            Email = email.ToLowerInvariant(),
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = passwordHash,
        };

    /// <summary>Created manually by an admin with a temporary password that must be changed.</summary>
    public static Member CreateByAdmin(string email, string firstName, string lastName, string passwordHash, Guid createdByUserId) =>
        new()
        {
            Email = email.ToLowerInvariant(),
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = passwordHash,
            MustChangePassword = true,
            CreatedByUserId = createdByUserId,
        };

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        SetUpdatedAt();
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        MustChangePassword = false;
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
}
