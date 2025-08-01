using MediatR;
using user.application.Interfaces;
using user.domain;
using user.domain.Entities;
using user.sharedkernel;

namespace user.application.Commands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repository;

    public CreateUserHandler(IUserRepository repository) => _repository = repository;

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var name = new UserName(request.FirstName, request.LastName);
        var user = new User(name);
        await _repository.AddAsync(user);
        return user.Id;
    }
}
