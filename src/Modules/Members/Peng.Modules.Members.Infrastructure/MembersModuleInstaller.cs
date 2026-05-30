using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peng.Modules.Members.Application;
using Peng.Modules.Members.Domain.Repositories;
using Peng.Modules.Members.Infrastructure.Persistence;
using Peng.Modules.Members.Infrastructure.Persistence.Repositories;
using Peng.Modules.Members.Infrastructure.Services;
using Peng.SharedKernel.Application;
using Peng.SharedKernel.Infrastructure;

namespace Peng.Modules.Members.Infrastructure;

public class MembersModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MembersDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("Default"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "members")));

        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IMembersUnitOfWork, MembersUnitOfWork>();
        services.AddScoped<IUserInfoProvider, MemberInfoProvider>();
        services.AddSingleton<IModuleDescriptor, MembersModuleDescriptor>();
    }
}
