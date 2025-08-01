using Microsoft.EntityFrameworkCore;
using user.domain.Entities;

namespace user.infrastructure.Persistence.DbContexts;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);

            // 👇 Configure UserName as an owned value object
            builder.OwnsOne(u => u.Name, name =>
            {
                name.Property(n => n.First).HasColumnName("FirstName").IsRequired();
                name.Property(n => n.Last).HasColumnName("LastName").IsRequired();
            });
        });
    }
}

