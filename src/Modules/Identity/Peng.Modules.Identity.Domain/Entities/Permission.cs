using Peng.SharedKernel.Domain;

namespace Peng.Modules.Identity.Domain.Entities;

public class Permission : Entity
{
    private Permission() { }

    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Module { get; private set; } = default!;

    public static Permission Create(string code, string name, string description, string module)
    {
        return new Permission { Code = code, Name = name, Description = description, Module = module };
    }
}
