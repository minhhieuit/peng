using Peng.SharedKernel.Infrastructure;

namespace Peng.API.Extensions;

internal static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpointGroups(this IEndpointRouteBuilder app)
    {
        var endpointGroups = typeof(Program).Assembly.GetTypes()
            .Where(t => typeof(IEndpointGroup).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IEndpointGroup>();

        foreach (var group in endpointGroups)
            group.MapEndpoints(app);

        return app;
    }
}
