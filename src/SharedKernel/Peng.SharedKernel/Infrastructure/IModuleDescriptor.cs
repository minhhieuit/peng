namespace Peng.SharedKernel.Infrastructure;

/// <summary>
/// Describes a module's metadata for documentation and onboarding purposes.
/// Each module must implement this interface to self-document its capabilities.
/// </summary>
public interface IModuleDescriptor
{
    string ModuleName { get; }
    string Description { get; }
    string Version { get; }
    IEnumerable<PermissionDescriptor> Permissions { get; }
    IEnumerable<FeatureDescriptor> Features { get; }
}

public record PermissionDescriptor(string Code, string Name, string Description, string Category);

public record FeatureDescriptor(string Name, string Description, IEnumerable<string> BusinessRules);
