using Microsoft.EntityFrameworkCore;

namespace Peng.API.Extensions;

internal static class DatabaseExtensions
{
    /// <summary>
    /// Discovers every <see cref="DbContext"/> registered via AddDbContext by looking for
    /// its <c>DbContextOptions&lt;T&gt;</c> registration. New modules are picked up
    /// automatically — no per-module wiring needed.
    /// </summary>
    public static IReadOnlyList<Type> GetRegisteredDbContextTypes(this IServiceCollection services) =>
        services
            .Where(d => d.ServiceType.IsGenericType
                        && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))
            .Select(d => d.ServiceType.GenericTypeArguments[0])
            .Distinct()
            .ToList();

    /// <summary>
    /// Applies any pending EF Core migrations for the given contexts at startup.
    /// Note: this only *applies* migrations that already exist — run
    /// <c>dotnet ef migrations add</c> after changing an entity to generate them.
    /// </summary>
    public static async Task MigrateDatabasesAsync(this IServiceProvider services, IEnumerable<Type> contextTypes)
    {
        using var scope = services.CreateScope();
        foreach (var contextType in contextTypes)
        {
            var context = (DbContext)scope.ServiceProvider.GetRequiredService(contextType);
            await context.Database.MigrateAsync();
        }
    }
}
