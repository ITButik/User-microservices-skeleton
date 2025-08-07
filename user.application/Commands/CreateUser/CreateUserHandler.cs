using Microsoft.Extensions.Logging;
using Shared.Application.DomainEvents;
using Shared.Application.Mediator;
using user.application.Interfaces;
using user.domain.Entities;
using user.sharedkernel.Domain;

namespace user.application.Commands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public CreateUserHandler(IUserRepository repository, ILogger<CreateUserHandler> logger, IDomainEventDispatcher domainEventDispatcher)
    {
        _repository = repository;
        _logger = logger;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Handling CreateUserCommand for FirstName: {FirstName}, LastName: {LastName}", request.FirstName, request.LastName);

            var name = new UserName(request.FirstName, request.LastName);
            var user = new User(name);

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            await _domainEventDispatcher.DispatchAsync(user.DomainEvents, cancellationToken);
            user.ClearDomainEvents();

            _logger.LogInformation("User created successfully with Id: {UserId}", user.Id);

            return user.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a user with FirstName: {FirstName}, LastName: {LastName}", request.FirstName, request.LastName);
            throw; // Re-throw the exception to propagate it
        }
    }
}
