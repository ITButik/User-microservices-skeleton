using Shared.Application;

namespace user.application.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, string FirstName, string LastName) : IRequest;
