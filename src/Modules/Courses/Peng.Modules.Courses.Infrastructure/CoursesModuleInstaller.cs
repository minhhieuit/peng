using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peng.Modules.Courses.Application;
using Peng.Modules.Courses.Domain.Repositories;
using Peng.Modules.Courses.Infrastructure.Persistence;
using Peng.Modules.Courses.Infrastructure.Persistence.Repositories;
using Peng.SharedKernel.Infrastructure;

namespace Peng.Modules.Courses.Infrastructure;

public class CoursesModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CoursesDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("Default"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "courses")));

        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<ICoursesUnitOfWork, CoursesUnitOfWork>();
        services.AddSingleton<IModuleDescriptor, CoursesModuleDescriptor>();
    }
}
