using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Peng.Modules.Identity.Application;
using Peng.Modules.Identity.Domain.Entities;

namespace Peng.Modules.Identity.Infrastructure.Persistence;

public class IdentityDbSeeder(IdentityDbContext context, ILogger<IdentityDbSeeder> logger)
{
    public async Task SeedAsync()
    {
        await context.Database.MigrateAsync();
        await SeedPermissionsAsync();
        await SeedRolesAsync();
        logger.LogInformation("Identity database seeded successfully.");
    }

    private async Task SeedPermissionsAsync()
    {
        // Load all existing codes in one query, then batch-insert only missing ones.
        // On conflict (unique index) ignore — handles concurrent startup race condition.
        var existingCodes = (await context.Permissions.Select(p => p.Code).ToListAsync()).ToHashSet();

        var toAdd = IdentityPermissions.All
            .Where(p => !existingCodes.Contains(p.Code))
            .Select(p => Permission.Create(p.Code, p.Name, p.Description, "Identity"))
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
            if (!await context.Roles.AnyAsync(r => r.Name == "Admin"))
            {
                var adminRole = Role.Create("Admin", "System administrator with full access", isSystem: true);
                var allPermissions = await context.Permissions.ToListAsync();
                foreach (var perm in allPermissions) adminRole.AssignPermission(perm);
                context.Roles.Add(adminRole);
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

    private static bool IsUniqueConstraintOrDeadlock(DbUpdateException ex)
    {
        var pgEx = ex.InnerException as PostgresException;
        return pgEx?.SqlState is "23505" or "40P01"; // unique_violation or deadlock_detected
    }
}
