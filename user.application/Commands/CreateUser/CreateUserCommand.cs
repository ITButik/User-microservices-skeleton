using Shared.Application.Mediator;
using user.domain.Entities;

namespace user.application.Commands.CreateUser
{
    public record CreateUserCommand(string FirstName, string LastName) : IRequest<Guid>;
}
