using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Peng.SharedKernel.Infrastructure;

public interface IModuleInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
