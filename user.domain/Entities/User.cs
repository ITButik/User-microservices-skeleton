using Shared.Domain;
using user.domain.Events;
using user.sharedkernel.Domain;

namespace user.domain.Entities
{
    public class User : Entity, IAggregateRoot
    {
        public Guid Id { get; set; }
        public UserName Name { get; private set; }

        public User() { }
        public User(UserName name)
        {
            Id = Guid.NewGuid();
            Name = name;

            AddDomainEvent(new UserRegisteredDomainEvent(Id, Name));
        }

        public void UpdateName(UserName name)
        {
            Name = name;
        }
    }
}
