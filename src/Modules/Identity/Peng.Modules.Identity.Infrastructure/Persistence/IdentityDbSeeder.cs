using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Peng.Modules.Courses.Application;
using Peng.Modules.Identity.Application;
using Peng.Modules.Identity.Domain.Entities;
using Peng.Modules.Members.Application;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Infrastructure.Persistence;

public class IdentityDbSeeder(
    IdentityDbContext context,
    IPasswordHasher passwordHasher,
    IConfiguration configuration,
    ILogger<IdentityDbSeeder> logger)
{
    public async Task SeedAsync()
    {
        await context.Database.MigrateAsync();
        await SeedPermissionsAsync();
        await SeedRolesAsync();
        await SeedAdminUserAsync();
        logger.LogInformation("Identity database seeded successfully.");
    }

    private async Task SeedPermissionsAsync()
    {
        // Load all existing codes in one query, then batch-insert only missing ones.
        // On conflict (unique index) ignore — handles concurrent startup race condition.
        var existingCodes = (await context.Permissions.Select(p => p.Code).ToListAsync()).ToHashSet();

        var allPermissions = IdentityPermissions.All
            .Select(p => (p.Code, p.Name, p.Description, Module: "Identity"))
            .Concat(CoursesPermissions.All
                .Select(p => (p.Code, p.Name, p.Description, Module: "Courses")))
            .Concat(MembersPermissions.All
                .Select(p => (p.Code, p.Name, p.Description, Module: "Members")));

        var toAdd = allPermissions
            .Where(p => !existingCodes.Contains(p.Code))
            .Select(p => Permission.Create(p.Code, p.Name, p.Description, p.Module))
            .ToList();

        if (toAdd.Count == 0) return;

        await context.Permissions.AddRangeAsync(toAdd);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintOrDeadlock(ex))
        {
            // Another instance seeded concurrently — safe to ignore, data is already there.
            context.ChangeTracker.Clear();
            logger.LogWarning("Seed conflict on Permissions (concurrent startup), skipping.");
        }
    }

    private async Task SeedRolesAsync()
    {
        try
        {
            var allPermissions = await context.Permissions.ToListAsync();
            var adminRole = await context.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Name == "Admin");

            if (adminRole is null)
            {
                adminRole = Role.Create("Admin", "System administrator with full access", isSystem: true);
                foreach (var perm in allPermissions) adminRole.AssignPermission(perm);
                context.Roles.Add(adminRole);
            }
            else
            {
                var assignedIds = adminRole.RolePermissions.Select(rp => rp.PermissionId).ToHashSet();
                foreach (var perm in allPermissions.Where(p => !assignedIds.Contains(p.Id)))
                    adminRole.AssignPermission(perm);
            }

            if (!await context.Roles.AnyAsync(r => r.Name == "Member"))
                context.Roles.Add(Role.Create("Member", "Default role for registered users", isSystem: true));

            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintOrDeadlock(ex))
        {
            context.ChangeTracker.Clear();
            logger.LogWarning("Seed conflict on Roles (concurrent startup), skipping.");
        }
    }

    private async Task SeedAdminUserAsync()
    {
        var adminEmail = configuration["Seed:AdminEmail"] ?? "admin@peng.dev";
        var adminPassword = configuration["Seed:AdminPassword"] ?? "Admin@1234";

        if (await context.Users.AnyAsync(u => u.Email == adminEmail)) return;

        var adminRole = await context.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Name == "Admin");

        var passwordHash = passwordHasher.Hash(adminPassword);
        var admin = User.Create(adminEmail, "Admin", "System", passwordHash);
        if (adminRole is not null) admin.AssignRole(adminRole);

        await context.Users.AddAsync(admin);
        try
        {
            await context.SaveChangesAsync();
            logger.LogInformation("Admin user seeded: {Email}", adminEmail);
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintOrDeadlock(ex))
        {
            context.ChangeTracker.Clear();
        }
    }

    private static bool IsUniqueConstraintOrDeadlock(DbUpdateException ex)
    {
        var pgEx = ex.InnerException as PostgresException;
        return pgEx?.SqlState is "23505" or "40P01"; // unique_violation or deadlock_detected
    }
}
