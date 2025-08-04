using MediatR;
using user.application.Interfaces;
using user.sharedkernel.Domain;

namespace user.application.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _repository;

    public UpdateUserHandler(IUserRepository repository) => _repository = repository;

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id);
        if (user is null)
            throw new KeyNotFoundException("User not found.");

        var name = new UserName(request.FirstName, request.LastName);
        user.UpdateName(name);
        await _repository.SaveChangesAsync();
    }
}
