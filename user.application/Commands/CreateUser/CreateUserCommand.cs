using Shared.Application;
using user.domain.Entities;

namespace user.application.Commands.CreateUser
{
    public record CreateUserCommand(string FirstName, string LastName) : IRequest<Guid>;
}
