using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Peng.Modules.Courses.Application;
using Peng.Modules.Courses.Infrastructure;
using Peng.Modules.Identity.Application.Commands.Login;
using Peng.Modules.Identity.Infrastructure;
using Peng.Modules.Members.Application;
using Peng.Modules.Members.Infrastructure;
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
            new MembersModuleInstaller(),
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
            cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(MembersModuleDescriptor).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CoursesModuleDescriptor).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(LoginCommand).Assembly);
        services.AddValidatorsFromAssembly(typeof(MembersModuleDescriptor).Assembly);
        services.AddValidatorsFromAssembly(typeof(CoursesModuleDescriptor).Assembly);
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, Peng.API.Infrastructure.CurrentUser>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings are not configured.");

        // Shared auth primitives — used by both the Identity (admin) and Members modules.
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IEmailSender, NoOpEmailSender>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Keep JWT claim names as-is ("sub", "email", ...) instead of remapping
                // them to the long WS-* URIs, so ctx.User.FindFirst("sub") works directly.
                options.MapInboundClaims = false;
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

        // Audience guard: admin tokens and member tokens cannot be used against
        // each other's endpoints, even if a token somehow carried extra claims.
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminToken", policy =>
                policy.RequireClaim(TokenTypes.ClaimName, TokenTypes.Admin));
            options.AddPolicy("MemberToken", policy =>
                policy.RequireClaim(TokenTypes.ClaimName, TokenTypes.Member));
        });
        return services;
    }
}
