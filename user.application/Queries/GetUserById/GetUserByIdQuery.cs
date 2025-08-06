using Shared.Application.Mediator;
using user.application.DTOs;

namespace user.application.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;
