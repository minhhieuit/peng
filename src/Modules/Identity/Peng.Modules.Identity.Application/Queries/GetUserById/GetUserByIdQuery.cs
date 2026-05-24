using Peng.Modules.Identity.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Identity.Application.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IQuery<UserDto>;
