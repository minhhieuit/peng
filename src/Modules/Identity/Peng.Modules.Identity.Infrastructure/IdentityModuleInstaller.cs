using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.Modules.Identity.Infrastructure.Persistence;
using Peng.Modules.Identity.Infrastructure.Persistence.Repositories;
using Peng.SharedKernel.Infrastructure;

namespace Peng.Modules.Identity.Infrastructure;

public class IdentityModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "identity")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IUnitOfWork, IdentityUnitOfWork>();
        services.AddScoped<IdentityDbSeeder>();
    }
}
