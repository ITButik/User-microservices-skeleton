using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.DomainEvents;
using Shared.Application.Mediator;
using Shared.Infrastructure.DependencyInjection;
using Shared.Infrastructure.DomainEvents;

namespace user.api.Extensions;

public static class SharedServiceCollectionExtensions
{
    public static IServiceCollection AddSharedApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        return services;
    }

    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddSharedInfrastructureServices(); // from Shared.Infrastructure.DependencyInjection
        return services;
    }
}
