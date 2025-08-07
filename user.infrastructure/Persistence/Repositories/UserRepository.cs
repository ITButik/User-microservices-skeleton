using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using user.application.Interfaces;
using user.domain.Entities;
using user.infrastructure.Exceptions;
using user.infrastructure.Persistence.DbContexts;

namespace user.infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(UserDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(User user)
    {
        try
        {
            _logger.LogInformation("Adding a new user with Id: {UserId}", user.Id);
            _context.Users.Add(user);
            //await _context.SaveChangesAsync();
            _logger.LogInformation("User with Id: {UserId} added successfully.", user.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a user with Id: {UserId}", user.Id);
            InfrastructureExceptionHandler.Handle(ex); // This will throw a custom exception
        }
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all users from the database.");
            return await _context.Users.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching all users.");
            InfrastructureExceptionHandler.Handle(ex); // This will throw a custom exception
            return Enumerable.Empty<User>(); // This line is unreachable but ensures method signature compliance
        }
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching user with Id: {UserId}", id);
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with Id: {UserId} not found.", id);
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching user with Id: {UserId}", id);
            InfrastructureExceptionHandler.Handle(ex); // This will throw a custom exception
            return null; // This line is unreachable but ensures method signature compliance
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            _logger.LogInformation("Saving changes to the database.");
            await _context.SaveChangesAsync();
            _logger.LogInformation("Changes saved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving changes to the database.");
            InfrastructureExceptionHandler.Handle(ex); // This will throw a custom exception
        }
    }
}

