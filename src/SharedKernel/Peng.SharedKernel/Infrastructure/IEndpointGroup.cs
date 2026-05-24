using Microsoft.AspNetCore.Routing;

namespace Peng.SharedKernel.Infrastructure;

public interface IEndpointGroup
{
    void MapEndpoints(IEndpointRouteBuilder app);
}
