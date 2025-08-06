using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Mediator;
using Shared.Infrastructure.DependencyInjection;

namespace user.api.Extensions;

public static class SharedServiceCollectionExtensions
{
    public static IServiceCollection AddSharedApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        return services;
    }

    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddSharedInfrastructureServices(); // from Shared.Infrastructure.DependencyInjection
        return services;
    }
}
