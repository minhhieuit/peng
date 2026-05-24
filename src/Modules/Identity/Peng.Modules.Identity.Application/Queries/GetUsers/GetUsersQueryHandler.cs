using Peng.Modules.Identity.Application.DTOs;
using Peng.Modules.Identity.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Queries.GetUsers;

internal sealed class GetUsersQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUsersQuery, PagedList<UserDto>>
{
    public async Task<Result<PagedList<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, total) = await userRepository.GetPagedAsync(request.Page, request.PageSize, request.Search, cancellationToken);
        var dtos = users.Select(u => new UserDto(u.Id, u.Email, u.FirstName, u.LastName, u.FullName,
            u.IsActive, u.CreatedAt,
            u.UserRoles.Select(ur => ur.Role?.Name ?? "").Where(r => !string.IsNullOrEmpty(r)),
            u.GetAllPermissions())).ToList();

        return Result.Success(new PagedList<UserDto>(dtos, request.Page, request.PageSize, total));
    }
}
