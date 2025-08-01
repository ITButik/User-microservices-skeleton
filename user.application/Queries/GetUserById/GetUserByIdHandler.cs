using MediatR;
using user.application.DTOs;
using user.application.Interfaces;

namespace user.application.Queries.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _repository;

    public GetUserByIdHandler(IUserRepository repository) => _repository = repository;

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id);
        return user is null ? null : new UserDto(user.Id, user.Name.First, user.Name.Last);
    }
}
