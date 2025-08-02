using Microsoft.EntityFrameworkCore;
using user.application.Interfaces;
using user.domain.Entities;
using user.infrastructure.Exceptions;
using user.infrastructure.Persistence.DbContexts;

namespace user.infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context) => _context = context;

    public async Task AddAsync(User user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            InfrastructureExceptionHandler.Handle(ex);
        }
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        try
        {
            return await _context.Users.ToListAsync();
        }
        catch (Exception ex)
        {
            InfrastructureExceptionHandler.Handle(ex);
        }

        return Enumerable.Empty<User>();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _context.Users.FindAsync(id);
        }
        catch (Exception ex)
        {
            InfrastructureExceptionHandler.Handle(ex);
        }

        return null;
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            InfrastructureExceptionHandler.Handle(ex);
        }
    }
}

