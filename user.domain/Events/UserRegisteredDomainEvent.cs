using Shared.Domain;
using user.sharedkernel.Domain;

namespace user.domain.Events;

public sealed class UserRegisteredDomainEvent : IDomainEvent
{
    public Guid Id { get; set; }
    public UserName Name { get; private set; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public UserRegisteredDomainEvent(Guid userId, UserName name)
    {
        Id = userId;
        Name = name;
    }
}

