using MediatR;
using user.application.DTOs;

namespace user.application.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;
