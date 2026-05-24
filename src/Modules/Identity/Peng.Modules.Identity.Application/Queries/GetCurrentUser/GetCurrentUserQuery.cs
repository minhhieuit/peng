using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IQuery<UserDto>;
