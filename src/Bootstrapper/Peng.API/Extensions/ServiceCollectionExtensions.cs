using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Peng.Modules.Courses.Application;
using Peng.Modules.Courses.Infrastructure;
using Peng.Modules.Identity.Application.Commands.Register;
using Peng.Modules.Identity.Infrastructure;
using Peng.Modules.Identity.Infrastructure.Settings;
using Peng.SharedKernel.Application;
using Peng.SharedKernel.Behaviors;
using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        var installers = new List<IModuleInstaller>
        {
            new IdentityModuleInstaller(),
            new CoursesModuleInstaller(),
        };

        foreach (var installer in installers)
            installer.Install(services, configuration);

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CoursesModuleDescriptor).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(RegisterCommand).Assembly);
        services.AddValidatorsFromAssembly(typeof(CoursesModuleDescriptor).Assembly);
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, Peng.API.Infrastructure.CurrentUser>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings are not configured.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services.AddAuthorization();
        return services;
    }
}
