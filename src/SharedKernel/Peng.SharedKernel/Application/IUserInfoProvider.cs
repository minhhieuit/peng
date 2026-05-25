namespace Peng.SharedKernel.Application;

public record UserInfo(Guid Id, string FullName, string Email);

public interface IUserInfoProvider
{
    Task<IReadOnlyDictionary<Guid, UserInfo>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
