using MediatR;
using Microsoft.Extensions.Logging;
using user.application.DTOs;
using user.application.Interfaces;

namespace user.application.Queries.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<GetUserByIdHandler> _logger;

    public GetUserByIdHandler(IUserRepository repository, ILogger<GetUserByIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Handling GetUserByIdQuery for UserId: {UserId}", request.Id);

            var user = await _repository.GetByIdAsync(request.Id);

            if (user is null)
            {
                _logger.LogWarning("User with Id {UserId} not found.", request.Id);
                return null;
            }

            _logger.LogInformation("User with Id {UserId} retrieved successfully.", request.Id);

            return new UserDto(user.Id, user.Name.First, user.Name.Last);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling GetUserByIdQuery for UserId: {UserId}", request.Id);
            throw;
        }
    }
}
