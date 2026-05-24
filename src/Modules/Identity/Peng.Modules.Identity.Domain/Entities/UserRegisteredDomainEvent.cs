using Peng.SharedKernel.Domain;

namespace Peng.Modules.Identity.Domain.Entities;

public record UserRegisteredDomainEvent(Guid UserId, string Email) : DomainEvent;
