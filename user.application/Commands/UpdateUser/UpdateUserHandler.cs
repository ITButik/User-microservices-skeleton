using Microsoft.Extensions.Logging;
using Shared.Application;
using user.application.Interfaces;
using user.sharedkernel.Domain;

namespace user.application.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<UpdateUserHandler> _logger;

    public UpdateUserHandler(IUserRepository repository, ILogger<UpdateUserHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Handling UpdateUserCommand for UserId: {UserId}", request.Id);

            var user = await _repository.GetByIdAsync(request.Id);
            if (user is null)
            {
                _logger.LogWarning("User with Id {UserId} not found.", request.Id);
                throw new KeyNotFoundException("User not found.");
            }

            var name = new UserName(request.FirstName, request.LastName);
            user.UpdateName(name);

            await _repository.SaveChangesAsync();
            _logger.LogInformation("User with Id {UserId} updated successfully.", request.Id);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Error updating user: {Message}", ex.Message);
            throw; // Re-throw to propagate the exception
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating user with Id {UserId}.", request.Id);
            throw; // Re-throw to propagate the exception
        }
    }
}
