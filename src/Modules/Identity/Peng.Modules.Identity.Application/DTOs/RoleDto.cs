namespace Peng.Modules.Identity.Application.DTOs;

public record RoleDto(
    Guid Id,
    string Name,
    string Description,
    bool IsSystem,
    DateTime CreatedAt,
    IEnumerable<PermissionDto> Permissions);

public record PermissionDto(
    Guid Id,
    string Code,
    string Name,
    string Description,
    string Module);
