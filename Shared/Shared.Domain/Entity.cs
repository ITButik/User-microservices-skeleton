namespace Shared.Domain;

public class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public List<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }   

}
