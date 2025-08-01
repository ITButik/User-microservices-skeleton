using user.domain.Entities;

namespace user.application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
    Task SaveChangesAsync();
}
