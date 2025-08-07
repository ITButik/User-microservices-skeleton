using Microsoft.EntityFrameworkCore;
using Shared.Application.DomainEvents;
using Shared.Domain;
using user.application.Interfaces;
using user.domain.Entities;

namespace user.infrastructure.Persistence.DbContexts;

public class UserDbContext(
    DbContextOptions<UserDbContext> options,
    IDomainEventDispatcher domainEventDispatcher) 
    : DbContext(options), IUserDbContext
{
    //public UserDbContext(DbContextOptions<UserDbContext> options, ) : base(options) { }

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // You can add custom logic here before saving changes, like auditing or validation
        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAync();

        return result;
    }

    private async Task PublishDomainEventsAync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(e => 
            {
                List<IDomainEvent> domainEvents = e.DomainEvents.ToList();
                e.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        await domainEventDispatcher.DispatchAsync(domainEvents);
    }
}

