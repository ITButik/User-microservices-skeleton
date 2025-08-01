using user.application.Interfaces;
using user.infrastructure.Persistence.DbContexts;
using user.domain.Entities;
using user.sharedkernel;

namespace user.infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context) => _context = context;

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var username = new UserName("shashi", "singh");
        return new User(username);
        //return await _context.Users.FindAsync(id);
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}

