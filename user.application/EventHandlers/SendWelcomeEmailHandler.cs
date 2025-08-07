using Shared.Domain;
using user.domain.Events;

namespace user.application.EventHandlers;

public sealed class SendWelcomeEmailHandler : IDomainEventHandler<UserRegisteredDomainEvent>
{
    //private readonly IEmailService _emailService;

    public SendWelcomeEmailHandler()
    {
    }

    public async Task HandleAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // TO DO : Send Email
        await Task.CompletedTask;
    }
}

